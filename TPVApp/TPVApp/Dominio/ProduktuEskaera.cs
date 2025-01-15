using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using TPVApp.Dominio;

namespace TPVApp
{
    internal class ProduktuEskaera
    {
        public virtual int Id { get; set; }
        public virtual int Eskaera_id {  get; set; }
        public virtual int ProduktuaIzena {  get; set; }
        public virtual int ProduktuaKop {  get; set; }
        public virtual float Prezioa {  get; set; }

        public virtual Eskaera Eskaera { get; set; }


        public static void ProduktuEskaerakErakutsi(DataGridView dataGridView1, int eskaeraId)
        {
            // Crear una instancia de la conexión
            Connection connection = new Connection();
            string query = "SELECT * FROM eskaeraproduktua WHERE erreserba_id = @eskaeraId";

            using (MySqlConnection konexioa = connection.GetConnection())
            {
                konexioa.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, konexioa))
                {
                    cmd.Parameters.AddWithValue("@eskaeraId", eskaeraId); // Filtrar por Eskaera_id

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataTable tabla1 = new DataTable();
                    dataAdapter.Fill(tabla1);
                    dataGridView1.DataSource = tabla1;
                }

                // Ajustar las columnas y filas del DataGridView
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }

    }
}
