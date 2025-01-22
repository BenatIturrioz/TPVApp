using System;
using System.Windows.Forms;
using System.IO;
using System;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using TPVApp.Dominio;
using System.Linq;

namespace TPVApp
{
    public partial class Form2 : Form
    {
        string mahaiainfo;
        string datainfo;
        string prezioainfo;
        private int erabiltzaileaId;
        private int eskaeraId;

        public Form2(int ErabiltzaileaId)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            erabiltzaileaId = ErabiltzaileaId;
         
        }
       
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                Eskaera.EskaerakErakutsi(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            eskaeraId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            ProduktuEskaera.ProduktuEskaerakErakutsi(dataGridView2, eskaeraId);

            mahaiaTextBox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            mahaiainfo = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            dataTextBox.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            datainfo = dataGridView1.CurrentRow.Cells[2].ToString();


            prezioTotalaTextBox.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            prezioainfo = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Generar el PDF
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "Cuenta_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");

                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                Paragraph title = new Paragraph("******* Charlie's *******", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph($"Mahaia: {mahaiaTextBox.Text}"));
                document.Add(new Paragraph($"Data: {dataTextBox.Text}"));
                document.Add(new Paragraph("-------------------------------------"));

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;

                table.AddCell("Produktua");
                table.AddCell("Kantitatea");
                table.AddCell("Prezioa");

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string producto = row.Cells["produktu_izena"].Value?.ToString() ?? "N/A";
                        string cantidad = row.Cells["produktuaKop"].Value?.ToString() ?? "N/A";
                        string precio = row.Cells["prezioa"].Value?.ToString() ?? "N/A";

                        table.AddCell(producto);
                        table.AddCell(cantidad);
                        table.AddCell(precio);
                    }
                }

                document.Add(table);

                document.Add(new Paragraph("-------------------------------------"));
                document.Add(new Paragraph($"Totala: {prezioTotalaTextBox.Text}"));

                document.Close();

                MessageBox.Show($"PDF generado con éxito: {filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Marcar la eskaera como ordainduta en la base de datos
                using (var session = NH.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            int erreserbaId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value); // Convertir el ID

                            // Buscar la eskaera por ID
                            var eskaera = session.Query<Eskaera>().FirstOrDefault(f => f.Erreserba_id == erreserbaId);

                            if (eskaera != null)
                            {
                                eskaera.Ordaindua = true; // Marcar como pagada
                                session.Update(eskaera); // Actualizar en la base de datos
                                transaction.Commit(); // Confirmar cambios
                                MessageBox.Show("La eskaera se ha marcado como pagada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la eskaera con el ID proporcionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("El ID de la eskaera debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Revertir transacción en caso de error
                            MessageBox.Show($"Error al actualizar la eskaera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Eskaera eskaera = new Eskaera();
            eskaera.Langilea_id = erabiltzaileaId;
            eskaera.Erreserba_id = ProduktuEskaera.SiguienteErreserbaId();
           
            Form4 form4 = new Form4(eskaera);
            form4.Show();
            this.Close();
        }
    }
}
