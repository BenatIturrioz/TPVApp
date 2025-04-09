using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVApp.Dominio
{
    internal class Erabiltzailea
    {
        public string ErabiltzaileIzena { get; set; }
        public string Pasahitza { get; set; }
        public int ErabiltzaileaId { get; set; }
        public string LangileaMota { get; set; }

        private Connection sqlConnection;

        public Erabiltzailea()
        {
            sqlConnection = new Connection();
        }

        public bool ValidarErabiltzailea()
        {
            try
            {
                using (MySqlConnection connection = sqlConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT id, langilea_mota FROM erronka1.erabiltzailea WHERE erabiltzaileIzena = @erabiltzaile AND pasahitza = @pasahitza";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@erabiltzaile", this.ErabiltzaileIzena);
                        command.Parameters.AddWithValue("@pasahitza", this.Pasahitza);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.ErabiltzaileaId = reader.GetInt32("id");
                                this.LangileaMota = reader["langilea_mota"].ToString();
                                return this.LangileaMota == "3";
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el usuario: " + ex.Message);
            }
        }
    }
}
