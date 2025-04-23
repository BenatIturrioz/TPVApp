using System;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
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
                MessageBox.Show("Errorea: " + ex.Message);
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "Kontua_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");

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
                table.AddCell("Kopurua");
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

                MessageBox.Show($"PDFa ongi sortu da: {filePath}", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (var session = NH.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            int erreserbaId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                            var eskaera = session.Query<Eskaera>().FirstOrDefault(f => f.Erreserba_id == erreserbaId);

                            if (eskaera != null)
                            {
                                eskaera.Ordaindua = true;
                                session.Update(eskaera);
                                transaction.Commit();
                                MessageBox.Show("Eskaera ordaindutzat markatu da.", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Eskaera.EskaerakErakutsi(dataGridView1);
                                Eskaera.EskaerakErakutsi(dataGridView2);
                            }
                            else
                            {
                                MessageBox.Show("Ezin izan da eskaera aurkitu emandako IDarekin.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Eskaeraren IDa zenbaki balioduna izan behar da.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Errorea eskaera eguneratzean: {ex.Message}", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea PDFa sortzerakoan: {ex.Message}\n{ex.StackTrace}", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) { }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

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

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eskaeraId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                using (var session = NH.OpenSession())
                {
                    var eskaera = session.Query<Eskaera>()
                                         .FirstOrDefault(eskaeraItem => eskaeraItem.Erreserba_id == eskaeraId);

                    if (eskaera != null)
                    {
                        Form5 form5 = new Form5(eskaera);
                        form5.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ezin izan da eskaera aurkitu emandako IDarekin.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Mesedez, hautatu eskaera bat datagrid-ean aldatzeko.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eskaeraId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                DialogResult result = MessageBox.Show(
                    "Ziur zaude eskaera hau ezabatu nahi duzula?",
                    "Ezabaketa Berretsi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        bool eliminacionExitosa = false;

                        using (var session = NH.OpenSession())
                        using (var transaction = session.BeginTransaction())
                        {
                            var produktuakEskaera = session.Query<ProduktuEskaera>()
                                .Where(pe => pe.ErreserbaId == eskaeraId)
                                .ToList();

                            foreach (var produktuaEskaera in produktuakEskaera)
                            {
                                var produktua = session.Query<Produktua>()
                                    .FirstOrDefault(p => p.Izena == produktuaEskaera.Produktu_izena);

                                if (produktua != null)
                                {
                                    produktua.Kantitatea += produktuaEskaera.ProduktuaKop;
                                    session.Update(produktua);
                                }

                                session.Delete(produktuaEskaera);
                            }

                            var eskaera = session.Query<Eskaera>()
                                .FirstOrDefault(eskaeraItem => eskaeraItem.Erreserba_id == eskaeraId);

                            if (eskaera != null)
                            {
                                session.Delete(eskaera);
                                session.Flush();
                                transaction.Commit();
                                eliminacionExitosa = true;
                            }
                            else
                            {
                                MessageBox.Show("Ez da eskaera aurkitu.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        if (eliminacionExitosa)
                        {
                            MessageBox.Show("Eskaera ezabatu da eta stock-a eguneratu da.", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridView1.ClearSelection();
                            dataGridView2.Rows.Clear();
                            Eskaera.EskaerakErakutsi(dataGridView1);
                            dataGridView2.Rows.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Errorea eskaera ezabatzerakoan: " + ex.Message, "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Mesedez, hautatu eskaera bat taulan ezabatzeko.", "Abisua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            eguraldia eguraldia = new eguraldia();
            eguraldia.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}
