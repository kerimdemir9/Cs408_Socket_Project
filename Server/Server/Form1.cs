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
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private List<Socket> _clientSockets = new List<Socket>();
        private Boolean _terminating = false;
        private Boolean _listening = false;
        
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Receive(Socket client)
        {
            
        }
        
        private void Accept()
        {
            while(_listening)
            {
                try
                {
                    Socket newClient = _serverSocket.Accept();
                    _clientSockets.Add(newClient);
                    richTextBox_logs.AppendText("A client is connected.\n");

                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
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
                        richTextBox_logs.AppendText("The socket stopped working.\n");
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
                textBox_message.Enabled = true;
                button_send.Enabled = true;
                
                Thread acceptThread = new Thread(Accept);

            }
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _listening = false;
            _terminating = true;
            Environment.Exit(0);
        }
    }
}