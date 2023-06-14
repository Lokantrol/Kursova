using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 8080);

        NetworkStream stream = client.GetStream();

        // Відправка повідомлення CONNECT
        byte[] connectPacket = { 0x01 };
        stream.Write(connectPacket, 0, connectPacket.Length);

        // Очікування повідомлення CONNACK
        byte[] response = new byte[1];
        int bytesRead = stream.Read(response, 0, response.Length);

        if (bytesRead > 0 && response[0] == 0x02)
        {
            Console.WriteLine("Connection established.");

            // Відправка повідомлення DISCON
            byte[] disconnectPacket = { 0x03 };
            stream.Write(disconnectPacket, 0, disconnectPacket.Length);

            // Очікування повідомлення DISCONACK
            bytesRead = stream.Read(response, 0, response.Length);

            if (bytesRead > 0 && response[0] == 0x04)
            {
                Console.WriteLine("Disconnected from the server.");
            }
        }
        Console.ReadLine();
        stream.Close();
        client.Close();
        Console.ReadLine();
    }
}
