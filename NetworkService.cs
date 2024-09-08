using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkService
{
    private TcpListener server;
    private TcpClient client;
    private NetworkStream stream;

    public void StartServer(int port)
    {
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Task.Run(() => AcceptClientAsync());
    }

    private async Task AcceptClientAsync()
    {
        while (true)
        {
            client = await server.AcceptTcpClientAsync();
            stream = client.GetStream();
            Task.Run(() => ReadMessagesAsync());
        }
    }

    public void ConnectToServer(string address, int port)
    {
        client = new TcpClient(address, port);
        stream = client.GetStream();
    }

    public void SendMessage(string message)
    {
        if (stream == null) throw new InvalidOperationException("No connection established.");
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    private async Task ReadMessagesAsync()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + message);
            }
        }
    }
}