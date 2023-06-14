using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            // Відправити повідомлення до сервера
            string message = textBox1.Text;
            SendMessageToServer(message);

            // Очистити поле введення
            textBox1.Text = "";
        }

        private void SendMessageToServer(string message)
        {
            // Надіслати повідомлення до сервера
            // Ваш код для надсилання повідомлення тут

            // Приклад надсилання повідомлення до сервера
            string response = "Відповідь від сервера";
            AddMessageToListBox(response);
        }

        private void AddMessageToListBox(string message)
        {
            // Додати повідомлення до listBox1
            listBox1.Items.Add(message);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }
    }
}
