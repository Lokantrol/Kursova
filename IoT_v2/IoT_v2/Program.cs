using System;
using System.Net.Sockets;
using System.Text;

public class IoTEmulator
{
    private TcpClient client;
    private NetworkStream stream;

    public void Connect(string serverIp, int serverPort)
    {
        try
        {
            client = new TcpClient();
            client.Connect(serverIp, serverPort);
            stream = client.GetStream();

            Console.WriteLine("Connected to server");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection error: " + ex.Message);
        }
    }

    public void SendData(string data)
    {
        try
        {
            byte[] sendData = Encoding.ASCII.GetBytes(data);
            stream.Write(sendData, 0, sendData.Length);

            Console.WriteLine("Sent data: " + data);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending data: " + ex.Message);
        }
    }

    public void ReceiveData()
    {
        try
        {
            byte[] receiveData = new byte[1024];
            int bytesRead = stream.Read(receiveData, 0, receiveData.Length);
            string response = Encoding.ASCII.GetString(receiveData, 0, bytesRead);

            Console.WriteLine("Received data: " + response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error receiving data: " + ex.Message);
        }
    }

    public void Disconnect()
    {
        try
        {
            stream.Close();
            client.Close();

            Console.WriteLine("Disconnected from server");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error disconnecting: " + ex.Message);
        }
    }
}
