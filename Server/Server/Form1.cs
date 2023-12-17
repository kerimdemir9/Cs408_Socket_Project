using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Server
{
    public partial class Form1 : Form
    {
        private int _id; // local id for each client
        private bool _listening;
        private bool _terminating;

        private readonly object
            _printLock =
                new object(); // so that only one thread can print to logs at a time -> avoid mix ups from different threads trying to print at the same time

        private readonly object
            _allNamesLock =
                new object(); // so that only one thread can access the name set at a time -> avoid conflicts between clients

        private readonly Socket _serverSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly object _printIf100Lock = new object(); // same as printLock for IF100 channel
        private readonly object _printSps101Lock = new object(); // same as printLock for SPS101 channel

        private readonly object
            _clientSocketsLock =
                new object(); // to avoid conflicts between threads trying to access clientSockets dictionary while another thread is modifying it

        private readonly HashSet<string>
            _allNames = new HashSet<string>(); // unique names list for checking if name exists already 

        private readonly Dictionary<int, string> _names = new Dictionary<int, string>(); // local id : name pair

        private readonly Dictionary<int, Socket>
            _clientSockets = new Dictionary<int, Socket>(); // local id : socket pair

        private readonly Dictionary<int, bool>
            _isSubscribedToSps101 = new Dictionary<int, bool>(); // local id : subscription status pair

        private readonly Dictionary<int, bool>
            _isSubscribedToIf100 = new Dictionary<int, bool>(); // local id : subscription status pair

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += Form1_FormClosing;
        }

        private void RemoveClient(Socket client, int clientId)
        {
            client.Close();
            lock (_clientSocketsLock)
            {
                _clientSockets.Remove(clientId);
            }

            if (_names[clientId] != null)
            {
                lock (_allNamesLock)
                {
                    _allNames.Remove(_names[clientId]);
                }

                // client is disconnected, so remove all information regarding that client
                _isSubscribedToIf100.Remove(clientId);
                _isSubscribedToSps101.Remove(clientId);
                _names.Remove(clientId);
            }
        }

        private void SendAck(Socket client, int clientId, string message)
        {
            try
            {
                client.Send(Encoding.Default.GetBytes(message));
            }
            catch (Exception)
            {
                lock (_printLock)
                {
                    var name = _names[clientId] != null ? _names[clientId] : clientId.ToString();
                    richTextBox_logs.AppendText(string.Concat("--> An exception occured while sending ACK to client ",
                        name, "!\n"));
                }
            }
        }

        private void HandleNameMessage(Socket client, int clientId, string message) // handle name sent by client
        {
            lock
                (_allNamesLock) // lock name set so no other client tries to enter a new name at the same time, to prevent conflicts
            {
                if (_allNames.Contains(message)) // if duplicate name is entered
                {
                    if (_names[clientId] != message)
                    {
                        lock (_printLock)
                        {
                            richTextBox_logs.AppendText(string.Concat("--> Client ", clientId,
                                " tried to enter name that is already taken. Closing connection.\n"));
                        }

                        SendAck(client, clientId, "DUPLICATE_USER");
                        RemoveClient(client, clientId);
                    }
                }
                else
                {
                    if (_names[clientId] != null) // if a client tries to change its username
                    {
                        _allNames.Remove(_names[clientId]); // remove existing name
                    }

                    _allNames.Add(message); // add new username
                    _names[clientId] = message;
                    lock (_printLock)
                    {
                        richTextBox_logs.AppendText(string.Concat("--> Client name: ", message, " registered.\n"));
                    }

                    SendAck(client, clientId, "USER_ADDED_SUCCESS");
                }
            }
        }

        private void HandleIf100Message(int clientId, string message)
        {
            try
            {
                lock (_printLock) // print to logs in the server
                {
                    richTextBox_logs.AppendText(string.Concat("--> ", _names[clientId],
                        " sent a message to IF100 channel: ", message, "\n"));
                }

                lock (_printIf100Lock) // print to the channel of IF100 in the server
                {
                    richTextBox_if100.AppendText(string.Concat("--> ", _names[clientId], ": ", message, "\n"));
                }

                lock (_clientSocketsLock) // send the message to all subscribed channels
                {
                    foreach (var clientSocket in _clientSockets)
                    {
                        if (_isSubscribedToIf100[clientSocket.Key]) // check if client is subscribed
                        {
                            clientSocket.Value.Send(Encoding.Default.GetBytes(string.Concat("MESSAGE:",
                                _names[clientId], ":IF100:", message)));
                        }
                    }
                }
            }
            catch (Exception)
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText(string.Concat("--> Client ", _names[clientId],
                        ": An exception occured while sending message to IF100 channel!\n"));
                }
            }
        }

        private void HandleSps101Message(int clientId, string message)
        {
            try
            {
                lock (_printLock) // same logic as above method
                {
                    richTextBox_logs.AppendText(string.Concat("--> ", _names[clientId],
                        " sent a message to SPS101 channel: ", message, "\n"));
                }

                lock (_printSps101Lock)
                {
                    richTextBox_sps101.AppendText(string.Concat("--> ", _names[clientId], ": ", message, "\n"));
                }

                lock (_clientSocketsLock)
                {
                    foreach (var clientSocket in _clientSockets)
                    {
                        if (_isSubscribedToSps101[clientSocket.Key])
                        {
                            clientSocket.Value.Send(Encoding.Default.GetBytes(string.Concat("MESSAGE:",
                                _names[clientId], ":SPS101:", message)));
                        }
                    }
                }
            }
            catch (Exception)
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText(string.Concat("--> Client ", _names[clientId],
                        ": An exception occured while sending message to SPS101 channel!\n"));
                }
            }
        }

        private void
            Receive(Socket client, int clientId) // receive messages sent from client // separate thread for each client
        {
            _isSubscribedToIf100[clientId] = false; // initially client is not subscribed to any channel
            _isSubscribedToSps101[clientId] = false; // initially client is not subscribed to any channel
            while (!_terminating && !(client.Poll(1000, SelectMode.SelectRead) && client.Available == 0)) // while connected and not terminating
            {
                try
                {
                    // receive a message from client
                    var buffer = new Byte[64];
                    client.Receive(buffer);
                    var incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf('\0'));

                    // classify the message
                    if (incomingMessage[0] == '0') // registering name
                    {
                        HandleNameMessage(client, clientId, incomingMessage.Substring(1));
                    }

                    if (incomingMessage[0] == '1') // subscription for IF100
                    {
                        var message = incomingMessage.Substring(1);
                        // bool result;
                        bool.TryParse(message, out var result);
                        if (_isSubscribedToIf100[clientId] !=
                            result) // if subscription status is different from the one in the server // normally it should be different, but still check to avoid unnecessary processing
                        {
                            _isSubscribedToIf100[clientId] = result;

                            if (result)
                            {
                                lock (_printLock)
                                {
                                    richTextBox_logs.AppendText(string.Concat("--> ", _names[clientId],
                                        " subscribed to IF100.\n"));
                                }

                                lock (_printIf100Lock)
                                {
                                    richTextBox_if100.AppendText(string.Concat("--> ", _names[clientId],
                                        " subscribed to IF100.\n"));
                                    SendAck(client, clientId, "SUBSCRIBED_SUCCESS:IF100");
                                }
                            }
                            else
                            {
                                lock (_printLock)
                                {
                                    richTextBox_logs.AppendText(string.Concat("--> ", _names[clientId],
                                        " unsubscribed from IF100.\n"));
                                }

                                lock (_printIf100Lock)
                                {
                                    richTextBox_if100.AppendText(string.Concat("--> ", _names[clientId],
                                        " unsubscribed from IF100.\n"));
                                    SendAck(client, clientId, "UNSUBSCRIBED_SUCCESS:IF100");
                                }
                            }
                        }
                    }

                    if (incomingMessage[0] == '2') // subscription for SPS101
                    {
                        var message = incomingMessage.Substring(1);
                        // bool result;
                        bool.TryParse(message, out var result);
                        if (_isSubscribedToSps101[clientId] != result)
                        {
                            _isSubscribedToSps101[clientId] = result;

                            if (result)
                            {
                                lock (_printLock)
                                {
                                    richTextBox_logs.AppendText(string.Concat("--> ", _names[clientId],
                                        " subscribed to SPS101.\n"));
                                }
                                lock (_printSps101Lock)
                                {
                                    richTextBox_sps101.AppendText(string.Concat("--> ", _names[clientId],
                                        " subscribed to SPS101.\n"));
                                    SendAck(client, clientId, "SUBSCRIBED_SUCCESS:SPS101");
                                }
                            }
                            else
                            {
                                lock (_printLock)
                                {
                                    richTextBox_logs.AppendText(string.Concat("--> ", _names[clientId],
                                        " unsubscribed from SPS101.\n"));
                                }
                                lock (_printSps101Lock)
                                {
                                    richTextBox_sps101.AppendText(string.Concat("--> ", _names[clientId],
                                        " unsubscribed from SPS101.\n"));
                                    SendAck(client, clientId, "UNSUBSCRIBED_SUCCESS:SPS101");
                                }
                            }
                        }
                    }

                    if (incomingMessage[0] == '3') // messages for IF100
                    {
                        if (_isSubscribedToIf100[clientId])
                        {
                            HandleIf100Message(clientId, incomingMessage.Substring(1));
                        }
                    }

                    if (incomingMessage[0] == '4') // messages for SPS101
                    {
                        if (_isSubscribedToSps101[clientId])
                        {
                            HandleSps101Message(clientId, incomingMessage.Substring(1));
                        }
                    }
                }
                catch (Exception)
                {
                    if (!(client.Poll(1000, SelectMode.SelectRead) && client.Available == 0))
                    {
                        break;
                    }
                }
            }

            if (!_terminating)
            {
                lock (_printLock)
                {
                    var name = _names != null ? _names[clientId] : clientId.ToString();
                    richTextBox_logs.AppendText(
                        string.Concat("--> Client ", name,
                            " has disconnected. Removing all information belonging to the client.\n"));
                }
            }

            RemoveClient(client, clientId);
        }

        private void Accept() // accepts connection from client
        {
            while (_listening)
            {
                try
                {
                    var newClient = _serverSocket.Accept();
                    lock (_clientSocketsLock)
                    {
                        _clientSockets[_id] = newClient;
                    }

                    lock (_printLock)
                    {
                        richTextBox_logs.AppendText("--> A client is connected.\n");
                    }

                    _names[_id] = null;
                    var tempId = _id;
                    var receiveThread = new Thread(() => Receive(newClient, tempId));
                    _id++;
                    receiveThread.Start();
                }
                catch
                {
                    if (_terminating)
                    {
                        _listening = false;
                    }
                    else
                    {
                        lock (_printLock)
                        {
                            richTextBox_logs.AppendText("--> Couldn't accept a new socket. Try Again.\n");
                        }
                    }
                }
            }
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(textBox_port.Text, out var serverPort))
                {
                    var endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                    _serverSocket.Bind(endPoint);
                    _serverSocket.Listen(10);

                    _listening = true;
                    _terminating = false;
                    button_listen.Enabled = false;

                    var acceptThread = new Thread(Accept);
                    acceptThread.Start();
                    lock (_printLock)
                    {
                        richTextBox_logs.AppendText(string.Concat("--> Started listening on port: ", serverPort, "\n"));
                    }
                }
                else
                {
                    lock (_printLock)
                    {
                        richTextBox_logs.AppendText("--> Please check the port number.\n");
                    }
                }
            }
            catch (Exception)
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText("--> A problem happened, please try again.\n");
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _listening = false;
            _terminating = true;
            Environment.Exit(0);
        }
    }
}