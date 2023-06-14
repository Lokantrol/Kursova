using System;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Отримати введені дані з textBox1
            string data = textBox1.Text;

            // Надіслати PUSH повідомлення до сервера
            SendMessageToServer(ControlByte.PUSH, data);
        }

        private void SendMessageToServer(ControlByte controlByte, string data)
        {
            // Надіслати повідомлення до сервера
            // Ваш код для надсилання повідомлення тут

            // Приклад надсилання повідомлення до сервера
            string message = GetFormattedMessage(controlByte, data);
            AddMessageToListBox("Надіслано: " + message);
        }

        private void AddMessageToListBox(string message)
        {
            // Додати повідомлення до listBox1
            listBox1.Items.Add(message);
        }

        private string GetFormattedMessage(ControlByte controlByte, string data)
        {
            // Форматувати повідомлення з контрольним байтом і даними
            return string.Format("{0}:{1}", ((byte)controlByte).ToString("X2"), data);
        }

        private enum ControlByte
        {
            CONNECT = 0x1,
            CONNACK = 0x2,
            DISCON = 0x3,
            DISCONACK = 0x4,
            PUSH = 0x5,
            PUSHACK = 0x6,
            PUSHNOACK = 0x7
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }

    static class Program
    {
        [STAThread]
        public static void main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }
    }
}