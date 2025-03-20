using System;

using System.IO;



using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPVApp.Dominio
{
    internal class Faktura
    {
        public static void FakturaSortu(string mahaiainfo, string datainfo, string perzioainfo, DataGridView dataGridView)
        {
            try
            {
                // Ruta del escritorio del usuario
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Crear nombre del archivo PDF
                string filePath = Path.Combine(desktopPath, "Cuenta_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");


                // Crear documento PDF
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                // Abrir el documento para agregar contenido
                document.Open();

                // Título
                Paragraph title = new Paragraph("******* Charlie's *******", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Información general
                document.Add(new Paragraph($"Mahaia: {mahaiainfo}"));
                document.Add(new Paragraph($"Data: {datainfo}"));
                document.Add(new Paragraph("-------------------------------------"));

                // Tabla
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;

                // Encabezado de la tabla
                table.AddCell("Produktua");
                table.AddCell("Kantitatea");
                table.AddCell("Prezioa");

                foreach (DataGridViewRow row in dataGridView.Rows)
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

                // Agregar la tabla al documento
                document.Add(table);

                // Totales
                document.Add(new Paragraph("-------------------------------------"));
                document.Add(new Paragraph($"Totala: {perzioainfo}"));

                // Cerrar el documento
                document.Close();

                // Mostrar mensaje de éxito
                MessageBox.Show($"PDF generado con éxito: {filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
