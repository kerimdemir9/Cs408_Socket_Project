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
        private int _id;
        private bool _listening;
        private bool _terminating;
        private readonly object _printLock = new object();
        private readonly object _allNamesLock = new object();
        private readonly Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly object _printIf100Lock = new object();
        private readonly object _printSps101Lock = new object();
        private readonly object _clientSocketsLock = new object();
        private readonly HashSet<string> _allNames = new HashSet<string>(); // unique names list for checking if name exists already 
        private readonly Dictionary<int, string> _names = new Dictionary<int, string>(); // id : name pair
        private readonly Dictionary<int, Socket> _clientSockets = new Dictionary<int, Socket>();
        private readonly Dictionary<int, bool> _isSubscribedToSps101 = new Dictionary<int, bool>();
        private readonly Dictionary<int, bool> _isSubscribedToIf100 = new Dictionary<int, bool>();

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += Form1_FormClosing;
        }

        private void HandleNameMessage(Socket client, int clientId, string message) // handle name sent by client
        {
            lock (_allNamesLock)
            {
                if (_allNames.Contains(message))
                {
                    lock (_printLock)
                    {
                        richTextBox_logs.AppendText(string.Concat("Client ", clientId,
                            " tried to enter name that is already taken. Closing connection.\n"));
                    }

                    client.Close();
                }
                else
                {
                    if (_names[clientId] != "null")
                    {
                        _allNames.Remove(_names[clientId]);
                    }
                    _allNames.Add(message);
                    _names[clientId] = message;
                    lock (_printLock)
                    {
                        richTextBox_logs.AppendText(string.Concat("Client name: ", message, " registered.\n"));
                    }
                }
            }
        }

        private void HandleIf100Message(int clientId, string message)
        {
            try
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText(string.Concat("Client ", _names[clientId], " sent message to IF100: ",
                        message, "\n"));
                }
                
                lock (_printIf100Lock)
                {
                    richTextBox_if100.AppendText(string.Concat("Client ", _names[clientId], " sent message to IF100: ",
                        message, "\n"));
                }

                lock (_clientSocketsLock)
                {
                    foreach (var clientSocket in _clientSockets)
                    {
                        if (_isSubscribedToIf100[clientSocket.Key])
                        {
                            clientSocket.Value.Send(Encoding.Default.GetBytes(message));
                        }
                    }
                }
            }
            catch (Exception)
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText("An exception occured while sending message to IF100 channel!\n");
                }
            }
        }

        private void HandleSps101Message(int clientId, string message)
        {
            try
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText(string.Concat("Client ", _names[clientId], " sent message to SPS101: ",
                        message, "\n"));
                }
                
                lock (_printSps101Lock)
                {
                    richTextBox_sps101.AppendText(string.Concat("Client ", _names[clientId], " sent message to IF100: ",
                        message, "\n"));
                }

                lock (_clientSocketsLock)
                {
                    foreach (var clientSocket in _clientSockets)
                    {
                        if (_isSubscribedToSps101[clientSocket.Key])
                        {
                            clientSocket.Value.Send(Encoding.Default.GetBytes(message));
                        }
                    }
                }
            }
            catch (Exception)
            {
                lock (_printLock)
                {
                    richTextBox_logs.AppendText("An exception occured while sending message to SPS101 channel!\n");
                }
            }
        }

        private void Receive(Socket client, int clientId) // receive messages sent from client
        {
            Boolean connected = true;

            while (connected && !_terminating)
            {
                try
                {
                    // receive a message from client
                    var buffer = new Byte[64];
                    client.Receive(buffer);
                    var incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf('\0'));

                    if (incomingMessage[0] == '0') // registering name
                    {
                        HandleNameMessage(client, clientId, incomingMessage.Substring(1));
                    }

                    if (incomingMessage[0] == '1') // subscription for IF100
                    {
                        var message = incomingMessage.Substring(1);
                        // bool result;
                        bool.TryParse(message, out var result);
                        if (_isSubscribedToIf100[clientId] != result)
                        {
                            _isSubscribedToIf100[clientId] = result;
                            lock (_printLock)
                            {
                                if (result)
                                {
                                    richTextBox_if100.AppendText(string.Concat(_names[clientId], " subscribed to IF100.\n"));
                                }
                                else
                                {
                                    richTextBox_if100.AppendText(string.Concat(_names[clientId],
                                        " unsubscribed from IF100.\n"));
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
                            lock (_printLock)
                            {
                                if (result)
                                {
                                    richTextBox_sps101.AppendText(string.Concat(_names[clientId],
                                        " subscribed to SPS101.\n"));
                                }
                                else
                                {
                                    richTextBox_sps101.AppendText(string.Concat(_names[clientId],
                                        " unsubscribed from SPS101.\n"));
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
                    // richTextBox_logs.AppendText(e.ToString() + "\n"); // error log
                    if (!_terminating)
                    {
                        richTextBox_logs.AppendText(
                            "A client has disconnected. Removing all information belonging to the client.\n");
                    }

                    client.Close();
                    lock (_clientSocketsLock)
                    {
                        _clientSockets.Remove(clientId);
                    }

                    if (_names[clientId] != "null")
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

                    connected = false;
                }
            }
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

                    richTextBox_logs.AppendText("A client is connected.\n");
                    _names[_id] = "null";
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
                        richTextBox_logs.AppendText("Couldn't accept a new socket. Try Again.\n");
                    }
                }
            }
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            try
            {
                // int serverPort;
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
                    richTextBox_logs.AppendText(String.Concat("Started listening on port: ", serverPort, "\n"));
                }
                else
                {
                    richTextBox_logs.AppendText("Please check the port number.\n");
                }
            }
            catch (Exception)
            {
                richTextBox_logs.AppendText("A problem happened, please try again.\n");
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