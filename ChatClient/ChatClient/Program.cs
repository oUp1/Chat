using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    class Program
    {
        private static int port = 8005; 
        private static string address = "10.5.114.83"; 
        static void Main(string[] args)
        {
            try
            {
                var ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                socket.Connect(ipPoint);
                Console.Write("Enter text: ");

                var message = Console.ReadLine();
                var data = Encoding.Unicode.GetBytes(message);

                socket.Send(data);

                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0; 

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                Console.WriteLine("Responce: " + builder.ToString());

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}