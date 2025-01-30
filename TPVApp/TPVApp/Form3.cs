using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TPVApp
{
    public partial class Form3 : Form
    {
        private TextBox messageField;
        private FlowLayoutPanel messageArea;
        private StreamWriter outWriter;
        private StreamReader inReader;
        private TcpClient clientSocket;

        public Form3()
        {
            InitializeComponent();
            InitializeUI();
            ConnectToServer();
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeUI()
        {
            messageArea = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false 
            };
            messageField = new TextBox
            {
                Dock = DockStyle.Bottom
            };
            messageField.KeyPress += MessageField_KeyPress;

            Controls.Add(messageArea);
            Controls.Add(messageField);
        }


        private void MessageField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) 
            {
                HandleSendMessage();
            }
        }

        private void HandleSendMessage()
        {
            string message = messageField.Text;
            if (!string.IsNullOrEmpty(message))
            {
                AddMessage(message, true); 
                SendMessage(message); 
                messageField.Clear();
            }
        }

        private void ConnectToServer()
        {
            try
            {
                clientSocket = new TcpClient("192.168.115.188", 5555);
                NetworkStream networkStream = clientSocket.GetStream();
                inReader = new StreamReader(networkStream);
                outWriter = new StreamWriter(networkStream, System.Text.Encoding.UTF8);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception e)
            {
                AddMessage("No se pudo conectar al servidor.", false);
                MessageBox.Show(e.Message);
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                string incomingMessage;
                while ((incomingMessage = inReader.ReadLine()) != null)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        AddMessage(incomingMessage, false);
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SendMessage(string message)
        {
            if (outWriter != null)
            {
                outWriter.WriteLine(message); 
                outWriter.Flush(); 
            }
        }

        private void AddMessage(string message, bool isSentByUser)
        {
            Label messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                MaximumSize = new System.Drawing.Size(200, 0),
                Padding = new Padding(10),
                BackColor = isSentByUser ? System.Drawing.Color.LightBlue : System.Drawing.Color.LightGreen,
                ForeColor = System.Drawing.Color.Black,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Margin = new Padding(5) 
            };

            FlowLayoutPanel messageBox = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Padding = new Padding(0)
            };

            messageBox.Controls.Add(messageLabel);
            messageArea.Controls.Add(messageBox);
            messageArea.ScrollControlIntoView(messageBox); 
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

