using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPServer.Helpers;
using TCPServer.Models;

namespace TCPServer.Services
{
    public class ClientHandler
    {
        private readonly TcpClient _client;

        public ClientHandler(TcpClient client)
        {
            _client = client;
        }

        public async Task HandleAsync()
        {
            try
            {
                using NetworkStream stream = _client.GetStream();

                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Client disconnected.");
                        break;
                    }

                    string encryptedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine($"Encrypted Request: {encryptedMessage}");

                    string message = EncryptionService.Decrypt(encryptedMessage);

                    Console.WriteLine($"Decrypted Request: {message}");

                    if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Client requested disconnect.");
                        break;
                    }

                    await ProcessRequest(stream, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _client.Close();
            }
        }

        private async Task ProcessRequest(NetworkStream stream, string message)
        {
            if (!MessageParser.TryParse(message, out string setName, out string key))
            {
                await SendMessage(stream, "EMPTY");
                await SendMessage(stream, "END");
                return;
            }

            if (!CollectionStore.Data.ContainsKey(setName))
            {
                await SendMessage(stream, "EMPTY");
                await SendMessage(stream, "END");
                return;
            }

            if (!CollectionStore.Data[setName].ContainsKey(key))
            {
                await SendMessage(stream, "EMPTY");
                await SendMessage(stream, "END");
                return;
            }

            int count = CollectionStore.Data[setName][key];

            for (int i = 0; i < count; i++)
            {
                await SendMessage(stream, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

                await Task.Delay(1000);
            }

            await SendMessage(stream, "END");
        }

        private async Task SendMessage(NetworkStream stream, string message)
        {
            string encrypted = EncryptionService.Encrypt(message);

            byte[] data = Encoding.UTF8.GetBytes(encrypted);

            await stream.WriteAsync(data, 0, data.Length);
        }
    }
}
