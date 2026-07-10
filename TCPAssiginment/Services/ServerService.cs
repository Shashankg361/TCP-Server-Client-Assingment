using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Services
{
    public class ServerService
    {
        private readonly TcpListener listener;

        public ServerService(string ip, int port)
        {
            listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public async Task StartAsync()
        {
            listener.Start();

            Console.WriteLine("================================");
            Console.WriteLine(" TCP Server Started ");
            Console.WriteLine("================================");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Client Connected");

                _ = Task.Run(() =>
                {
                    var handler = new ClientHandler(client);
                    return handler.HandleAsync();
                });
            }
        }
    }
}
