using System;
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
            Console.WriteLine("Source Send: " + Encoding.UTF8.GetString(message));
        }

        public string ReceiveMessage()
        {
            byte[] message = new byte[30];
            tcpSocket.Receive(message);
            Console.WriteLine("Dest Receive: " + Encoding.UTF8.GetString(message));

            return Encoding.UTF8.GetString(message);
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
