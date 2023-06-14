using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ServerApplication
{
    public partial class ServForm : Form
    {
        private TcpListener tcpListener;
        private TcpClient connectedClient;
        private NetworkStream networkStream;
        private Thread listenThread;
        private bool isListening;

        public ServForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            isListening = false;
        }

        private void StartListening()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 1234);
                tcpListener.Start();

                isListening = true;

                UpdateStatusLabel("Listening for clients...");

                while (isListening)
                {
                    connectedClient = tcpListener.AcceptTcpClient();
                    networkStream = connectedClient.GetStream();

                    Thread clientThread = new Thread(HandleClientConnection);
                    clientThread.Start();
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Socket Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleClientConnection()
        {
            try
            {
                // Read CONNECT packet from the client
                byte[] receiveBuffer = new byte[1024];
                int bytesRead = networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                string connectPacket = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

                // Parse the CONNECT packet
                string[] connectFields = connectPacket.Split(',');
                string protocolName = connectFields[0];
                string myName = connectFields[1];
                string secName = connectFields[2];
                int keepAlive = int.Parse(connectFields[3]);

                // Display client connected message in the list box
                UpdateListBox($"Client connected: {myName}");

                // Send CONNACK packet to the client
                string connAckPacket = "CONNACK,1";
                byte[] connAckBytes = Encoding.ASCII.GetBytes(connAckPacket);
                networkStream.Write(connAckBytes, 0, connAckBytes.Length);

                // Loop to receive and process PUSH packets from the client
                while (isListening)
                {
                    bytesRead = networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    string pushPacket = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

                    // Parse the PUSH packet
                    string[] pushFields = pushPacket.Split(',');
                    string pushData = pushFields[1];

                    // Display received data in the list box
                    UpdateListBox($"Received data: {pushData}");

                    // Send PUSHACK packet to the client
                    string pushAckPacket = "PUSHACK,1";
                    byte[] pushAckBytes = Encoding.ASCII.GetBytes(pushAckPacket);
                    networkStream.Write(pushAckBytes, 0, pushAckBytes.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Clean up the client connection
                networkStream.Close();
                connectedClient.Close();
            }
        }

        private void StopListening()
        {
            isListening = false;

            if (tcpListener != null)
                tcpListener.Stop();

            if (connectedClient != null)
            {
                networkStream.Close();
                connectedClient.Close();
            }

            UpdateStatusLabel("Not listening");
        }

        private void UpdateStatusLabel(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatusLabel), status);
            }
            else
            {
                statusLabel.Text = status;
            }
        }

        private void UpdateListBox(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateListBox), message);
            }
            else
            {
                listBox1.Items.Add(message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connectedClient != null && connectedClient.Connected)
            {
                string message = textBox1.Text;

                // Створити PUSH пакет з даними повідомлення
                string pushPacket = $"PUSH,{message}";

                try
                {
                    // Відправити PUSH пакет клієнту
                    byte[] pushBytes = Encoding.ASCII.GetBytes(pushPacket);
                    networkStream.Write(pushBytes, 0, pushBytes.Length);

                    // Вивести повідомлення в listBox1
                    UpdateListBox($"Message sent: {message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No connected client.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isListening)
            {
                listenThread = new Thread(StartListening);
                listenThread.Start();

                UpdateStatusLabel("Listening...");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopListening();
            UpdateStatusLabel("Listening STOP");
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (connectedClient != null && connectedClient.Connected)
            {
                string message = textBox1.Text;

                // Створити PUSH пакет з даними повідомлення
                string pushPacket = $"PUSH,{message}";

                try
                {
                    // Відправити PUSH пакет клієнту
                    byte[] pushBytes = Encoding.ASCII.GetBytes(pushPacket);
                    networkStream.Write(pushBytes, 0, pushBytes.Length);

                    // Вивести повідомлення в listBox1
                    UpdateListBox($"Message sent: {message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No connected client.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
