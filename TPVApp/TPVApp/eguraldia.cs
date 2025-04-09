using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TPVApp
{
    public partial class eguraldia : Form
    {
        private string localFile = "downloaded_output.xml";
        private RichTextBox richTextBox;

        public eguraldia()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void eguraldia_Load(object sender, EventArgs e)
        {
            // Botón para descargar
            Button btnDownload = new Button
            {
                Text = "Descargar Predicción",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(180, 40)
            };
            btnDownload.Click += BtnDownload_Click;
            this.Controls.Add(btnDownload);

            // RichTextBox para mostrar el tiempo
            richTextBox = new RichTextBox
            {
                Name = "richTextBoxTiempo",
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(this.ClientSize.Width - 40, this.ClientSize.Height - 100),
                Multiline = true,
                ReadOnly = true,
                Font = new System.Drawing.Font("Segoe UI", 11),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(richTextBox);
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            string ftpHost = "127.0.0.1";
            string ftpUser = "edit";
            string ftpPassword = "edit";
            string remoteFile = "output.xml";

            try
            {
                string uri = $"ftp://{ftpHost}/{remoteFile}";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fs = new FileStream(localFile, FileMode.Create))
                {
                    responseStream.CopyTo(fs);
                }

                MostrarPrediccion(localFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar: {ex.Message}");
            }
        }

        private void MostrarPrediccion(string path)
        {
            try
            {
                XDocument doc = XDocument.Load(path);
                StringBuilder sb = new StringBuilder();

                var dias = doc.Descendants("dia");

                foreach (var dia in dias)
                {
                    string fecha = dia.Element("fecha")?.Value;
                    string temperatura = dia.Element("temperatura_media")?.Value;
                    string precipitacion = dia.Element("prob_precipitacion_media")?.Value;

                    sb.AppendLine($"📅 {fecha}");
                    sb.AppendLine($"🌡️ Temp: {temperatura} °C");
                    sb.AppendLine($"☔ Lluvia: {Math.Round(Convert.ToDouble(precipitacion))}%");

                    var cielos = dia.Elements("estado_cielo")
                        .Where(x => x.Attribute("periodo") != null)
                        .GroupBy(x => x.Attribute("periodo").Value)
                        .OrderBy(x => x.Key);

                    foreach (var grupo in cielos)
                    {
                        string periodo = grupo.Key;
                        string descripcion = grupo.FirstOrDefault()?.Value;
                        sb.AppendLine($"☁️ Cielo ({periodo}): {descripcion}");
                    }

                    // Si hay un único <estado_cielo> sin periodo
                    var cieloUnico = dia.Elements("estado_cielo")
                        .Where(x => x.Attribute("periodo") == null)
                        .FirstOrDefault();

                    if (cieloUnico != null)
                    {
                        sb.AppendLine($"☁️ Cielo: {cieloUnico.Value}");
                    }

                    sb.AppendLine(new string('-', 40));
                }

                richTextBox.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar predicción: {ex.Message}");
            }
        }
    }
}
