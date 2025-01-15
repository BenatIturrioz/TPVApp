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
            if (e.KeyChar == (char)13) // Si se presiona la tecla Enter
            {
                HandleSendMessage();
            }
        }

        private void HandleSendMessage()
        {
            string message = messageField.Text;
            if (!string.IsNullOrEmpty(message))
            {
                AddMessage(message, true); // Mostrar mensaje enviado
                SendMessage(message);      // Enviar mensaje al servidor
                messageField.Clear();
            }
        }

        private void ConnectToServer()
        {
            try
            {
                clientSocket = new TcpClient("localhost", 5555);
                NetworkStream networkStream = clientSocket.GetStream();
                inReader = new StreamReader(networkStream);
                outWriter = new StreamWriter(networkStream, System.Text.Encoding.UTF8);

                // Hilo para recibir mensajes
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
                    // Actualizar la UI en el hilo principal
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
                outWriter.WriteLine(message); // Enviar el mensaje al servidor usando WriteLine
                outWriter.Flush(); // Asegúrate de que el mensaje se envíe inmediatamente
            }
        }

        private void AddMessage(string message, bool isSentByUser)
        {
            // Crear la etiqueta para el mensaje
            Label messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                MaximumSize = new System.Drawing.Size(200, 0),
                Padding = new Padding(10)
            };

            // Estilo para los mensajes enviados por el usuario
            if (isSentByUser)
            {
                messageLabel.BackColor = System.Drawing.Color.LightBlue;
                messageLabel.ForeColor = System.Drawing.Color.Black;
                messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            }
            else // Mensajes recibidos
            {
                messageLabel.BackColor = System.Drawing.Color.LightGreen;
                messageLabel.ForeColor = System.Drawing.Color.Black;
                messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            }

            // Crear el contenedor para el mensaje
            FlowLayoutPanel messageBox = new FlowLayoutPanel
            {
                FlowDirection = isSentByUser ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
                Dock = DockStyle.Top
            };
            messageBox.Controls.Add(messageLabel);

            // Añadir un espaciador vacío para el salto de línea
            FlowLayoutPanel spacePanel = new FlowLayoutPanel
            {
                Height = 10, // Puedes ajustar la altura del espacio
                Width = 0
            };

            // Agregar el mensaje y el espaciador al panel principal
            messageArea.Controls.Add(messageBox);
            messageArea.Controls.Add(spacePanel);
        }
    }
}


