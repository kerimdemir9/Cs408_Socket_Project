using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private int _id = 0;
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private List<Socket> _clientSockets = new List<Socket>();
        private Boolean _terminating = false;
        private Boolean _listening = false;
        private List<string> _names = new List<string>();
        private HashSet<string> _allNames = new HashSet<string>();
        private object _namesLock = new object();

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Receive(Socket client, int clientId)
        {
            Boolean connected = true;

            while (connected && !_terminating)
            {
                try
                {
                    var buffer = new Byte[64];
                    client.Receive(buffer);
                    var incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    
                    lock (_namesLock)
                    {
                        if (_allNames.Contains(incomingMessage))
                        {
                            richTextBox_logs.AppendText(String.Concat("Client ", clientId,
                                " tried to enter name that is already taken. Closing connection.\n"));
                            client.Close();
                        }
                        else
                        {
                            _allNames.Add(incomingMessage);
                            _names[clientId] = incomingMessage;
                            richTextBox_logs.AppendText(String.Concat("Client name: ", incomingMessage, " received.\n"));
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    // richTextBox_logs.AppendText(e.ToString() + "\n");
                    if (!_terminating)
                    {
                        richTextBox_logs.AppendText("A client has disconnected.\n");
                    }

                    client.Close();
                    _clientSockets.Remove(client);
                    connected = false;
                }
            }
        }

        private void Accept()
        {
            while (_listening)
            {
                try
                {
                    var newClient = _serverSocket.Accept();
                    _clientSockets.Add(newClient);
                    richTextBox_logs.AppendText("A client is connected.\n");
                    _names.Add(String.Concat("Client ", _id.ToString(), " haven't entered name!"));
                    var temp = _id;
                    var receiveThread = new Thread(() => Receive(newClient, temp));
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
                        richTextBox_logs.AppendText("Couldn't accept a new socket.\n");
                    }
                }
            }
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (int.TryParse(textBox_port.Text, out serverPort))
            {
                var endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                _serverSocket.Bind(endPoint);
                _serverSocket.Listen(10);

                _listening = true;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _listening = false;
            _terminating = true;
            Environment.Exit(0);
        }
    }
}