using TCPCLient.Services;

ClientService client = new ClientService("127.0.0.1", 5000);

await client.StartAsync();