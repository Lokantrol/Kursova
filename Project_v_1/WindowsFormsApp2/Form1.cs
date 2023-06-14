using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class ServerForm : Form
    {
        private List<string> messageList;

        public ServerForm()
        {
            InitializeComponent();
            messageList = new List<string>();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            // Запустити сервер IoT
            StartServer();
        }

        private void StartServer()
        {
            // Почати прийом повідомлень від IoT клієнтів
            // Ваш код для прийому повідомлень тут

            // Приклад отримання повідомлення від клієнта та відображення його у listBox1
            string message = "Повідомлення від клієнта";
            AddMessageToListBox(message);

            // Надіслати CONNACK після отримання CONNECT
            SendMessageToClient(ControlByte.CONNACK, "1");
        }

        private void AddMessageToListBox(string message)
        {
            // Додати повідомлення до списку та відобразити його у listBox1
            messageList.Add(message);
            listBox1.Items.Add(message);
        }

        private void SendMessageToClient(ControlByte controlByte, string data)
        {
            // Надіслати повідомлення до клієнта
            // Ваш код для надсилання повідомлення тут

            // Приклад надсилання повідомлення до клієнта
            string message = GetFormattedMessage(controlByte, data);
            AddMessageToListBox("Надіслано: " + message);
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
        private void ProcessMessage(string message)
        {
            // Розбити повідомлення на контрольний байт та дані
            string[] parts = message.Split(':');
            if (parts.Length != 2)
            {
                // Неправильний формат повідомлення
                return;
            }

            byte controlByteValue;
            if (!byte.TryParse(parts[0], System.Globalization.NumberStyles.HexNumber, null, out controlByteValue))
            {
                // Неправильне значення контрольного байта
                return;
            }

            ControlByte controlByte = (ControlByte)controlByteValue;
            string data = parts[1];

            // Обробка повідомлення в залежності від контрольного байта
            switch (controlByte)
            {
                case ControlByte.CONNECT:
                    // Обробка CONNECT повідомлення
                    // Ваш код для обробки CONNECT повідомлення тут
                    message = GetFormattedMessage(controlByte, data);
                    AddMessageToListBox("Підключено до сервера ");
                    break;
                case ControlByte.CONNACK:
                    // Обробка CONNACK повідомлення
                    // Ваш код для обробки CONNACK повідомлення тут
                    message = GetFormattedMessage(controlByte, data);
                    AddMessageToListBox("Підключився клієнт ");
                    break;
                case ControlByte.DISCON:
                    // Обробка DISCON повідомлення
                    // Ваш код для обробки DISCON повідомлення тут
                    message = GetFormattedMessage(controlByte, data);
                    AddMessageToListBox("Відключено від сервера ");
                    break;
                case ControlByte.DISCONACK:
                    message = GetFormattedMessage(controlByte, data);
                    AddMessageToListBox("Відключено від сервера ");
                    break;
                case ControlByte.PUSH:
                    // Обробка PUSH повідомлення
                    // Ваш код для обробки PUSH повідомлення тут
                    break;
                case ControlByte.PUSHACK:
                    // Обробка PUSHACK повідомлення
                    // Ваш код для обробки PUSHACK повідомлення тут
                    break;
                case ControlByte.PUSHNOACK:
                    // Обробка PUSHNOACK повідомлення
                    // Ваш код для обробки PUSHNOACK повідомлення тут
                    break;
                default:
                    // Невідомий контрольний байт
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            SendMessageToClient(ControlByte.CONNACK, text);
        }
    }

    static class Program
    {
        [STAThread]
        public static void main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());
        }
    }
}