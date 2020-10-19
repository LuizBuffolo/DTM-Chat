using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WPF___Chat.Models
{
    class Connection
    {
        private Socket tcpSocket = null;
        public string tipo { get; private set; }
        public bool isConnected { get; private set; }
        private IPEndPoint clientEndpoint;
        private IPEndPoint listenerEndpoint;
        private IPEndPoint broadCast = new IPEndPoint(IPAddress.Broadcast, 30678);
        private IPEndPoint broadCastEndpoint = new IPEndPoint(IPAddress.Any, 30678);

        private UdpClient udpClient = new UdpClient();
        private UdpClient udpListener = null;
        private TcpClient tcpClient = null;
        private TcpListener tcpListener = null;

        public void StartUdp()
        {
            ClientUdp();
            ListenerUdp();

        }

        public void ClientUdp()
        {
            byte[] send = Encoding.ASCII.GetBytes("<<<<UDP CONECTED>>>>");
            try
            {
                udpClient.Send(send, send.Length, broadCast);
                tipo = "Client";
                listenerEndpoint = new IPEndPoint(IPAddress.Parse(GetLocalIPAddress()), 12345);
            }
            catch
            {

            } 
        }

        public void ListenerUdp()
        {
            try
            {

                byte[] msg = new byte[1024];
                byte[] send = Encoding.ASCII.GetBytes("<<<<UDP AFTER CONECTED>>>>");
                udpListener = new UdpClient(broadCastEndpoint);

                msg = udpListener.Receive(ref broadCastEndpoint);
                

                if (Encoding.UTF8.GetString(msg) == "<<<<UDP CONECTED>>>>")
                {
                    tipo = "Listener";
                    clientEndpoint = new IPEndPoint(broadCastEndpoint.Address, 12345);
                }

            }
            catch
            {
                //clientEndpoint = new IPEndPoint(broadCastEndpoint.Address, 12345);
                tipo = "Client";
            }
            
        }



        public void Stop()
        {
            SendMessage("<<<<The another PC was Stopped the connection>>>>");
            tcpSocket.Shutdown(SocketShutdown.Both);
            isConnected = false;
        }

        public void StopOtherside()
        {
            tcpSocket.Shutdown(SocketShutdown.Both);
            isConnected = false;
        }

        public void SendMessage(string chat)
        {
            try
            {
                byte[] message = Encoding.ASCII.GetBytes(chat);
                tcpSocket.Send(message);
                //Console.WriteLine("Source Send: " + Encoding.UTF8.GetString(message));
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is SocketException
                                    || ex is ObjectDisposedException)
            {
                Console.WriteLine($"Error: {ex}, {ex.Message}");
            }

        }

        public string ReceiveMessage()
        {
            try
            { 
                string stringMessage;
                string result;
                byte[] message = new byte[124];
                List<char> shortMessage = new List<char>();

                tcpSocket.Receive(message);
                stringMessage = Encoding.UTF8.GetString(message);

                int x = 0;
                while (stringMessage[x] != '\0')
                {
                    shortMessage.Add(stringMessage[x]);
                    x++;
                }

                result = new string(shortMessage.ToArray());

                if (result == "<<<<The another PC was Stopped the connection>>>>")
                { 
                    StopOtherside();
                    return result;
                }
                else
                {
                    return result;
                }  
            }
             catch (Exception ex) when(ex is ArgumentNullException
                                        || ex is SocketException
                                        || ex is ObjectDisposedException)
            {
                Console.WriteLine($"Error: {ex}, {ex.Message}");
                return null;
            }
    }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public void Client()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(listenerEndpoint.Address, 12345);

                tcpSocket = tcpClient.Client;

                isConnected = true;
            }

            catch
            {
                isConnected = false;
            }

        }

        public void Listener()
        {
            
            if(clientEndpoint != null)
            {
                tcpListener = new TcpListener(clientEndpoint);
                tcpListener.Start();
                tcpSocket = tcpListener.AcceptSocket();

                isConnected = true;
            }
        }
    }
}
