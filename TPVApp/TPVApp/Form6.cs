using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPVApp.Dominio;

namespace TPVApp
{
    
        public partial class Form6 : Form
        {
            // Agregar una propiedad para almacenar el objeto Eskaera
            private Eskaera eskaera;
            private int mota;

            // Modificar el constructor para recibir el objeto Eskaera
            public Form6(Eskaera eskaera, int mota)
            {
                InitializeComponent();
                this.eskaera = eskaera; // Almacenar el objeto en la propiedad
                this.mota = mota;
            }

        private void Form6_Load(object sender, EventArgs e)
        {
            List<Produktua> productos = Produktua.ProduktuaErakutsi(mota);

            // Crear un DataGridView
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AllowUserToAddRows = false // Evitar que los usuarios agreguen filas manualmente
            };

            // Agregar columnas al DataGridView
            dataGridView.Columns.Add("Id", "ID");
            dataGridView.Columns.Add("Izena", "Nombre");
            dataGridView.Columns.Add("ErosketaPrezioa", "Precio");
            dataGridView.Columns.Add("Kantitatea", "Cantidad Disponible");

            DataGridViewTextBoxColumn seleccionarCantidadColumn = new DataGridViewTextBoxColumn
            {
                Name = "SeleccionarCantidad",
                HeaderText = "Cantidad a Elegir",
                ValueType = typeof(int)
            };
            dataGridView.Columns.Add(seleccionarCantidadColumn);

            // Agregar filas con los datos de los productos
            foreach (var producto in productos)
            {
                dataGridView.Rows.Add(producto.Id, producto.Izena, producto.ErosketaPrezioa, producto.Kantitatea, 0);
            }

            // Manejo de validación de datos en el DataGridView
            dataGridView.CellValidating += (s, ev) =>
            {
                if (ev.ColumnIndex == dataGridView.Columns["SeleccionarCantidad"].Index)
                {
                    // Validar el valor ingresado en la celda
                    int cantidadDisponible = Convert.ToInt32(dataGridView.Rows[ev.RowIndex].Cells["Kantitatea"].Value);
                    if (int.TryParse(ev.FormattedValue.ToString(), out int cantidadElegida))
                    {
                        if (cantidadElegida > cantidadDisponible)
                        {
                            MessageBox.Show("La cantidad seleccionada no puede ser mayor que la disponible.");
                            ev.Cancel = true; // Cancelar la edición si la cantidad es inválida
                        }
                        else if (cantidadElegida < 0)
                        {
                            MessageBox.Show("La cantidad seleccionada no puede ser negativa.");
                            ev.Cancel = true; // Cancelar la edición si la cantidad es negativa
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, ingrese un número válido.");
                        ev.Cancel = true; // Cancelar la edición si no es un número
                    }
                }
            };

            // Agregar el DataGridView al formulario
            this.Controls.Add(dataGridView);

            // Crear un botón de confirmación
            Button confirmarButton = new Button
            {
                Text = "Confirmar Selección",
                Dock = DockStyle.Bottom
            };

            // Manejo del evento Click del botón
            confirmarButton.Click += (s, ev) =>
            {
                StringBuilder seleccionResumen = new StringBuilder("Productos seleccionados:\n");
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    int productoId = Convert.ToInt32(row.Cells["Id"].Value);
                    string nombre = row.Cells["Izena"].Value.ToString();
                    float prezioa = Convert.ToSingle(row.Cells["ErosketaPrezioa"].Value);
                    int cantidadElegida = Convert.ToInt32(row.Cells["SeleccionarCantidad"].Value);

                    if (cantidadElegida > 0)
                    {
                        seleccionResumen.AppendLine($"- {nombre} (ID: {productoId}): {cantidadElegida} unidades");
                        ProduktuEskaera.gordeProduktuEskaera(eskaera, nombre, cantidadElegida, prezioa);
                    }
                }
                MessageBox.Show(seleccionResumen.ToString(), "Resumen de Selección");

                Form5 form5 = new Form5(eskaera); // Pasar el objeto eskaera al Form5
                form5.Show();

            };

            // Agregar el botón al formulario
            this.Controls.Add(confirmarButton);
        }

    }




}
