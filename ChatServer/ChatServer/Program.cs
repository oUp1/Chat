using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        private static int port = 8005; 
        public static void Main(string[] args)
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(GetLocalIPAddress()), port);
            var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                Console.WriteLine("Running...");

                while (true)
                {
                    var handler = listenSocket.Accept();
                    var builder = new StringBuilder();
                    var bytes = 0; 
                    var data = new byte[256]; 

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    string message = "Done";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static string GetLocalIPAddress()
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
    }
}