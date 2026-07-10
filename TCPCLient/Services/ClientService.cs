using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPCLient.Services
{
    public class ClientService
    {
        private readonly string _ip;
        private readonly int _port;

        public ClientService(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public async Task StartAsync()
        {
            using TcpClient client = new();

            await client.ConnectAsync(_ip, _port);

            Console.WriteLine("Connected to Server");

            using NetworkStream stream = client.GetStream();

            while (true)
            {
                Console.Write("\nEnter Message (or 'exit'): ");

                string? message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                    continue;

                string encrypted = EncryptionService.Encrypt(message);

                Console.WriteLine($"Encrypted Request: {encrypted}");

                byte[] data = Encoding.UTF8.GetBytes(encrypted);

                await stream.WriteAsync(data, 0, data.Length);

                if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        return;

                    string encryptedResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    string response = EncryptionService.Decrypt(encryptedResponse);

                    if (response == "END")
                    {
                        break;
                    }

                    Console.WriteLine(response);
                }
            }
        }
    }
}
