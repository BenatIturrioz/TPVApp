using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Drawing;
using System.Globalization;


namespace TPVApp
{
    public partial class eguraldia : Form
    {
        private string localFile = "downloaded_output.xml";
        private Panel panelResultados;

        public eguraldia()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void eguraldia_Load(object sender, EventArgs e)
        {
            Button btnDownload = new Button
            {
                Text = "Descargar Predicción",
                Location = new Point(20, 20),
                Size = new Size(200, 40)
            };
            btnDownload.Click += BtnDownload_Click;
            this.Controls.Add(btnDownload);

            panelResultados = new Panel
            {
                Location = new Point(20, 80),
                AutoScroll = true,
                Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(panelResultados);
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

                MostrarPrediccionVisual(localFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar: {ex.Message}");
            }
        }

        private void MostrarPrediccionVisual(string path)
        {
            try
            {
                panelResultados.Controls.Clear();

                XDocument doc = XDocument.Load(path);
                var dias = doc.Descendants("dia");

                int y = 10;

                foreach (var dia in dias)
                {
                    string fecha = dia.Element("fecha")?.Value;
                    string temperaturaStr = dia.Element("temperatura_media")?.Value;
                    string precipitacion = dia.Element("prob_precipitacion_media")?.Value;
                    double precipitacionVal = double.TryParse(precipitacion, NumberStyles.Any, CultureInfo.InvariantCulture, out double p) ? p : 0;
                    double temperatura = double.TryParse(temperaturaStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double t) ? t : 0;



                    string descripcion = $"📅 {fecha}\n🌡️ Temp: {temperatura} °C\n☔ Lluvia: {Math.Round(precipitacionVal)}%\n";

                    var cielos = dia.Elements("estado_cielo")
                        .Where(x => x.Attribute("periodo") != null)
                        .GroupBy(x => x.Attribute("periodo").Value)
                        .OrderBy(x => x.Key);

                    foreach (var grupo in cielos)
                    {
                        string periodo = grupo.Key;
                        string desc = grupo.FirstOrDefault()?.Value;
                        descripcion += $"☁️ Cielo ({periodo}): {desc}\n";
                    }

                    var cieloUnico = dia.Elements("estado_cielo")
                        .Where(x => x.Attribute("periodo") == null)
                        .FirstOrDefault();
                    if (cieloUnico != null)
                    {
                        descripcion += $"☁️ Cielo: {cieloUnico.Value}\n";
                    }

                    // Crear RichTextBox
                    RichTextBox txt = new RichTextBox
                    {
                        Text = descripcion,
                        Location = new Point(80, y),
                        Size = new Size(panelResultados.Width - 100, 110),
                        ReadOnly = true,
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.None,
                        BackColor = SystemColors.Control
                    };

                    // Seleccionar imagen según temperatura
                    PictureBox img = new PictureBox
                    {
                        Size = new Size(60, 60),
                        Location = new Point(10, y + 20),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    string rutaImagenCalor = Path.Combine(Application.StartupPath, "Resources", "calor.png");
                    string rutaImagenFrio = Path.Combine(Application.StartupPath, "Resources", "frio.png");
                    if (temperatura >= 20)
                        img.Image = Image.FromFile(rutaImagenCalor);
                    else
                        img.Image = Image.FromFile(rutaImagenFrio);

                    PictureBox imgLluvia = new PictureBox
                    {
                        Size = new Size(60, 60),
                        Location = new Point(10, y + 80), // debajo de la otra imagen
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    string rutaLluvia = Path.Combine(Application.StartupPath, "Resources", "lluvia.png");
                    string rutaSeco = Path.Combine(Application.StartupPath, "Resources", "seco.png");
                    if (precipitacionVal > 50)
                        imgLluvia.Image = Image.FromFile(rutaLluvia);
                    else
                        imgLluvia.Image = Image.FromFile(rutaSeco);

                    panelResultados.Controls.Add(imgLluvia);

                    // Añadir al panel
                    panelResultados.Controls.Add(img);
                    panelResultados.Controls.Add(txt);

                    y += 130;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar predicción: {ex.Message}");
            }
        }
    }
}
