using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace TPVApp.Dominio
{
    internal class Eskaera
    {
        public virtual int Id {  get; set; }
        public virtual int Langilea_id { get; set; }
        public virtual int Erreserba_id { get; set; }
        public virtual int Mahaia {  get; set; }
        public virtual DateTime Data {  get; set; }
        public virtual float PrezioTotala { get; set; }
        public virtual byte? Ordaindua {  get; set; }

        public static void EskaerakErakutsi(DataGridView dataGridView1)
        {
            // Crear una instancia de la conexión
            Connection connection = new Connection();
            string query = "SELECT erreserba_id, mahaia_id, data, prezioTotala FROM eskaera";

            using (MySqlConnection konexioa = connection.GetConnection())
            {
                konexioa.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, konexioa))
                {
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
