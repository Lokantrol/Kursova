using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();

        Console.WriteLine("Waiting for incoming connections...");

        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Client connected.");

        NetworkStream stream = client.GetStream();

        // Очікування повідомлення CONNECT
        byte[] connectPacket = new byte[1];
        int bytesRead = stream.Read(connectPacket, 0, connectPacket.Length);

        if (bytesRead > 0 && connectPacket[0] == 0x01)
        {
            Console.WriteLine("Connection established.");

            // Відправка повідомлення CONNACK
            byte[] connackPacket = { 0x02 };
            stream.Write(connackPacket, 0, connackPacket.Length);
            Console.ReadLine();
            // Очікування повідомлення DISCON
            byte[] disconnectPacket = new byte[1];
            bytesRead = stream.Read(disconnectPacket, 0, disconnectPacket.Length);

            if (bytesRead > 0 && disconnectPacket[0] == 0x03)
            {
                Console.WriteLine("Received disconnect request.");

                // Відправка повідомлення DISCONACK
                byte[] disconackPacket = { 0x04 };
                stream.Write(disconackPacket, 0, disconackPacket.Length);

                Console.WriteLine("Disconnected from the client.");
            }
        }
        Console.ReadKey();
        stream.Close();
        client.Close();
        if (Console.ReadLine() == "close") ;
        {
            server.Stop();
        }
        Console.ReadLine();
    }
}
