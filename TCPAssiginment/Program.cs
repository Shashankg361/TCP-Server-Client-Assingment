using TCPServer.Services;

ServerService server = new ServerService("127.0.0.1", 5000);

await server.StartAsync();