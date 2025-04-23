using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate;
using TPVApp.Dominio;

namespace TPVApp
{
    public partial class Form6 : Form
    {
        // Eskaera objektua gordetzeko propietatea
        private Eskaera eskaera;
        private int mota;

        // Eraikitzailea: Eskaera objektua eta mota jasotzen ditu
        public Form6(Eskaera eskaera, int mota)
        {
            InitializeComponent();
            this.eskaera = eskaera; // Objektua esleitu
            this.mota = mota;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // Aukeratzeko produktu erabilgarriak lortu
            List<Produktua> productos = Produktua.ProduktuaErakutsi(mota);

            // DataGridView sortu
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AllowUserToAddRows = false // Erabiltzaileek eskuz errenkadak gehitzea saihestu
            };

            // Zutabeak gehitu
            dataGridView.Columns.Add("Id", "ID");
            dataGridView.Columns.Add("Izena", "Izena");
            dataGridView.Columns.Add("ErosketaPrezioa", "Prezioa");
            dataGridView.Columns.Add("Kantitatea", "Eskuragarri");

            DataGridViewTextBoxColumn seleccionarCantidadColumn = new DataGridViewTextBoxColumn
            {
                Name = "SeleccionarCantidad",
                HeaderText = "Hautatutako Kantitatea",
                ValueType = typeof(int)
            };
            dataGridView.Columns.Add(seleccionarCantidadColumn);

            // Aukeratutako kantitateak datu-basetik jaso
            using (ISession session = NH.OpenSession())
            {
                var eskaeraProductos = session.QueryOver<ProduktuEskaera>()
                                              .Where(pe => pe.ErreserbaId == eskaera.Erreserba_id)
                                              .List<ProduktuEskaera>();

                // Produktuak eta lehenagotik aukeratutako kantitateak gehitu
                foreach (var producto in productos)
                {
                    var productoEskaera = eskaeraProductos.FirstOrDefault(pe => pe.Produktu_izena == producto.Izena);
                    int cantidadSeleccionada = productoEskaera?.ProduktuaKop ?? 0;

                    dataGridView.Rows.Add(producto.Id, producto.Izena, producto.ErosketaPrezioa, producto.Kantitatea, cantidadSeleccionada);
                }
            }

            // Balidazioa
            dataGridView.CellValidating += (s, ev) =>
            {
                if (ev.ColumnIndex == dataGridView.Columns["SeleccionarCantidad"].Index)
                {
                    int cantidadDisponible = Convert.ToInt32(dataGridView.Rows[ev.RowIndex].Cells["Kantitatea"].Value);
                    if (int.TryParse(ev.FormattedValue.ToString(), out int cantidadElegida))
                    {
                        if (cantidadElegida > cantidadDisponible)
                        {
                            MessageBox.Show("Hautatutako kantitatea ezin da erabilgarri dagoena baino handiagoa izan.");
                            ev.Cancel = true;
                        }
                        else if (cantidadElegida < 0)
                        {
                            MessageBox.Show("Kantitatea ezin da negatiboa izan.");
                            ev.Cancel = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mesedez, zenbaki baliozko bat sartu.");
                        ev.Cancel = true;
                    }
                }
            };

            // Gehitu DataGridView form-ari
            this.Controls.Add(dataGridView);

            // Berrespen botoia sortu
            Button confirmarButton = new Button
            {
                Text = "Hautapena Berretsi",
                Dock = DockStyle.Bottom
            };

            confirmarButton.Click += (s, ev) =>
            {
                StringBuilder seleccionResumen = new StringBuilder("Hautatutako produktuak:\n");
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    int productoId = Convert.ToInt32(row.Cells["Id"].Value);
                    string nombre = row.Cells["Izena"].Value.ToString();
                    float prezioa = Convert.ToSingle(row.Cells["ErosketaPrezioa"].Value);
                    int cantidadElegida = row.Cells["SeleccionarCantidad"].Value != null
                                ? Convert.ToInt32(row.Cells["SeleccionarCantidad"].Value)
                                : 0;
                    if (cantidadElegida >= 0)
                    {
                        seleccionResumen.AppendLine($"- {nombre} (ID: {productoId}): {cantidadElegida} unitate");
                        ProduktuEskaera.gordeProduktuEskaera(eskaera, nombre, cantidadElegida, prezioa);
                    }
                }
                MessageBox.Show(seleccionResumen.ToString(), "Hautapenaren Laburpena");

                Form5 form5 = new Form5(eskaera); // Eskaera pasatu Form5-era
                form5.Show();
                this.Close();
            };

            this.Controls.Add(confirmarButton);
        }
    }
}
