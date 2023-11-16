using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
                }
                catch
                {
                    _clientSocket.Close();
                    Environment.Exit(0);
                }
            }
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            var message = textBox_name.Text;
            var buffer = Encoding.Default.GetBytes(message);
            _clientSocket.Send(buffer);
        }
    }
}