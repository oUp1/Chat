using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Chat.Web.Services
{
    public class MessageService
    {
        private IPEndPoint _ipPoint;
        private Socket _socket;
        public MessageService(int port, string address)
        {
            _ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task SendMessage(string text)
        {
            try
            {
                _socket.Connect(_ipPoint);
                var data = Encoding.Unicode.GetBytes(text);

                _socket.Send(data);

                data = new byte[256];
                var builder = new StringBuilder();

                int bytes = 0;

                do
                {
                    bytes = _socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (_socket.Available > 0);
                Console.WriteLine("ответ сервера: " + builder.ToString());

                // закрываем сокет
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch { }
        }
    }
}

