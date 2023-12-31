﻿using System;
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
            var message = textBox_name.Text;
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _connected = false;
            Environment.Exit(0);
        }
    }
}