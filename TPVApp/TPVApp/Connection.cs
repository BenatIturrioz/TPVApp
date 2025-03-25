using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVApp
{
    internal class Connection
    {
        private readonly string connectionString = "server='192.168.115.158';port='3306';user id='2Taldea'; password = '2Taldea'; database = 'erronka2';SslMode = 'none'";

        public MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la conexión: " + ex.Message);
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
