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
                                Eskaera.EskaerakErakutsi(dataGridView1);
                                Eskaera.EskaerakErakutsi(dataGridView2);
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eskaeraId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                using (var session = NH.OpenSession())
                {
                    // Buscar la Eskaera por ID en la base de datos
                    var eskaera = session.Query<Eskaera>()
                                         .FirstOrDefault(eskaeraItem => eskaeraItem.Erreserba_id == eskaeraId);

                    if (eskaera != null)
                    {
                        // Abrir el Form5 con la Eskaera seleccionada
                        Form5 form5 = new Form5(eskaera);
                        form5.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la Eskaera con el ID proporcionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una Eskaera en el DataGrid para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eskaeraId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                DialogResult result = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar esta Eskaera?",
                    "Confirmar Eliminación",
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
                            // Recuperamos los productos asociados a la Eskaera
                            var produktuakEskaera = session.Query<ProduktuEskaera>()
                                .Where(pe => pe.ErreserbaId == eskaeraId)
                                .ToList();

                            // Iteramos sobre los productos de la Eskaera
                            foreach (var produktuaEskaera in produktuakEskaera)
                            {
                                // Recuperamos el producto
                                var produktua = session.Query<Produktua>()
                                    .FirstOrDefault(p => p.Izena == produktuaEskaera.Produktu_izena);

                                if (produktua != null)
                                {
                                    // Aumentamos la cantidad del producto en stock
                                    produktua.Kantitatea += produktuaEskaera.ProduktuaKop;
                                    session.Update(produktua);  // Actualizamos el producto
                                }

                                session.Delete(produktuaEskaera);
                            }

                            // Ahora, eliminamos la Eskaera en sí
                            var eskaera = session.Query<Eskaera>()
                                .FirstOrDefault(eskaeraItem => eskaeraItem.Erreserba_id == eskaeraId);

                            if (eskaera != null)
                            {
                                session.Delete(eskaera);  // Eliminamos la Eskaera
                                session.Flush();  // Forzamos la eliminación en la base de datos
                                transaction.Commit();  // Confirmamos la transacción
                                eliminacionExitosa = true;
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la Eskaera.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        if (eliminacionExitosa)
                        {
                            MessageBox.Show("Eskaera eliminada y stock actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Limpiamos las selecciones y actualizamos las tablas
                            dataGridView1.ClearSelection();
                            dataGridView2.Rows.Clear(); // Limpiamos el DataGridView de los productos
                            Eskaera.EskaerakErakutsi(dataGridView1); // Actualizamos el DataGridView de Eskaera
                            dataGridView2.Rows.Clear(); // Aseguramos que el DataGridView de productos está limpio
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la Eskaera: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una Eskaera en la tabla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            eguraldia eguraldia = new eguraldia();
            eguraldia.Show();
        }
    }





}