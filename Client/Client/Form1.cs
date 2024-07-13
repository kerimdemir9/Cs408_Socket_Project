using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private Socket _clientSocket;
        private Boolean _connected;
        private Boolean _if100Sub;
        private Boolean _sps101Sub;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += Form1_FormClosing;
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = "127.0.0.1";
            int port;
            if (int.TryParse(textBox_port.Text, out port))
            {
                try
                {
                    _clientSocket.Connect(ip, port);
                    button_connect.Enabled = false;
                    _connected = true;
                    button_send.Enabled = true;

                    var isSocketAlive = new Thread(IsConnected);
                    var receiveMessages = new Thread(Listen);
                    receiveMessages.Start();    
                    isSocketAlive.Start();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    _clientSocket.Close();
                    Environment.Exit(0);
                }
            }
        }

        private void Listen()
        {
            while(_connected) 
            {
                try
                {
                    var buffer = new Byte[64];
                    _clientSocket.Receive(buffer);
                    var message = Encoding.Default.GetString(buffer);
                    message = message.Substring(0, message.IndexOf('\0'));

                    string[] messageArray = message.Split(':');
                    var lesson = messageArray[2];
                    if (lesson == "SPS101" && _sps101Sub)
                    {
                        var name = messageArray[1];
                        var broadMessage = messageArray[3];
                        var print = "--> " + name + ": " + broadMessage + "\n";
                        richTextBox_sps101.AppendText(print);
                    }
                    if (lesson == "IF100" && _if100Sub)
                    {
                        var name = messageArray[1];
                        var broadMessage = messageArray[3];
                        var print = "--> " + name + ": " + broadMessage + "\n";
                        richTextBox_if100.AppendText(print);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
        }

        private void IsConnected()
        {
            while (true)
            {
                try
                {
                    _connected = !(_clientSocket.Poll(1, SelectMode.SelectRead) && _clientSocket.Available == 0);
                    if (!_connected)
                    {
                        button_connect.Enabled = true;
                        button_send.Enabled = false;
                    }
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            var message = "0" + textBox_name.Text;
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _connected = false;
            Environment.Exit(0);
        }


        private void button_sub_if100_Click(object sender, EventArgs e)
        {
            var message = "";
            if (button_sub_if100.Text == "subscribe")
            {
                message += "1true";
                button_sub_if100.Text = "Unsubscribe";
                _if100Sub = true;
            }
            else
            {
                message += "1false";
                button_sub_if100.Text = "subscribe";
                _if100Sub = false;
            }
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }

        private void button_send_if100_Click(object sender, EventArgs e)
        {
            var message = "3" + textBox_if100.Text;
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }

        private void button_sub_sps101_Click(object sender, EventArgs e)
        {
            var message = "";
            if (button_sub_sps101.Text == "subscribe")
            {
                message += "2true";
                button_sub_sps101.Text = "Unsubscribe";
                _sps101Sub = true;
            }
            else
            {
                message += "2false";
                button_sub_sps101.Text = "subscribe";
                _sps101Sub = false;
            }
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }

        private void button_send_sps101_Click(object sender, EventArgs e)
        {
            var message = "4" + textBox_sps101.Text;
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }
    }
}