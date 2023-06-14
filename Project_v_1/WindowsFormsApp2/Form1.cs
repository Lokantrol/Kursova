using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ClientApplication
{
    public partial class ClientForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 1234);

            stream = client.GetStream();

            SendMessage("CONNECT,Client,security,60"); 

            listBox1.Items.Add("Під'єднано до сервера");

            // Очікування повідомлення CONNACK
            buffer = new byte[1024];
            stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendMessage("PUSH," + textBox1.Text);

            listBox1.Items.Add("Повідомлення надіслано");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendMessage("DISCON");

            // Очікування повідомлення DISCONACK
            buffer = new byte[1024];
            stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, null);
        }

        private void SendMessage(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = stream.EndRead(ar);
                if (bytesRead > 0)
                {
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessMessage(response);

                    // Продовжуємо очікування повідомлень
                    buffer = new byte[1024];
                    stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, null);
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок з'єднання
                MessageBox.Show("Помилка при отриманні повідомлення: " + ex.Message);
            }
        }

        private void ProcessMessage(string message)
        {
            // Обробка отриманих повідомлень
            if (message.StartsWith("CONNACK"))
            {
                listBox1.Items.Add("Отримано підтвердження підключення");
            }
            else if (message.StartsWith("PUSH"))
            {
                string[] parts = message.Split(',');
                if (parts.Length >= 2)
                {
                    string data = parts[1];
                    listBox1.Items.Add("Отримано дані: " + data);

                    SendMessage("PUSHACK");
                }
            }
            else if (message.StartsWith("DISCONACK"))
            {
                listBox1.Items.Add("Відключено від сервера");

                // Закриття з'єднання та потоку
                stream.Close();
                client.Close();
            }
        }
    }
}