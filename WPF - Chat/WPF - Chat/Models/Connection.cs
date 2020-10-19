using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WPF___Chat.Models
{
    class Connection
    {
        private Socket tcpSocket = null;

        private IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Parse("192.168.10.10"), 30789);
        private IPEndPoint listenerEndpoint = new IPEndPoint(IPAddress.Parse("192.168.10.10"), 30789);

        private TcpClient tcpClient = null;
        private TcpListener tcpListener = null;

        public void Start()
        {
            tcpSocket = Client();

            if (tcpSocket == null)
            {
                tcpSocket = Listener();
            }
        }

        public void SendMessage(string chat)
        {
            byte[] message = Encoding.ASCII.GetBytes(chat);
            tcpSocket.Send(message);
            //Console.WriteLine("Source Send: " + Encoding.UTF8.GetString(message));
        }

        public string ReceiveMessage()
        {
            string stringMessage;
            string result;
            byte[] message = new byte[124];
            List<char> shortMessage = new List<char>();

            tcpSocket.Receive(message);
            stringMessage = Encoding.UTF8.GetString(message);

            int x = 0;
            while(stringMessage[x] != '\0') {
                shortMessage.Add(stringMessage[x]);
                x++;
            }

            result = new string(shortMessage.ToArray());

            return result;
        }

        public Socket Client()
        {
            try
            {

                tcpClient = new TcpClient();
                tcpClient.Connect(listenerEndpoint.Address, 30789);

                tcpSocket = tcpClient.Client;

                return tcpSocket;
            }

            catch
            {
                return null;
            }

        }

        public Socket Listener()
        {
            tcpListener = new TcpListener(clientEndpoint);
            tcpListener.Start();
            tcpSocket = tcpListener.AcceptSocket();

            return tcpSocket;
            // tcpListener.Stop();
        }
    }
}
