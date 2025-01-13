using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using NHibernate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TPVApp.Dominio;

namespace TPVApp
{
    public partial class Form2 : Form
    {

        private NHibernate.Cfg.Configuration myConfiguration;
        private ISessionFactory mySessionFactory;
        private ISession mySession;
        private int erabiltzaileaId;

        public Form2(int ErabiltzaileaId)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            erabiltzaileaId = ErabiltzaileaId;
        }

        private void LoadDataGridView()
        {
            using (var session = mySessionFactory.OpenSession())
            {
                var eskaerak = session.Query<Eskaera>().ToList();
                dataGridView1.DataSource = eskaerak; // Asume que dataGridView1 es el nombre de tu DataGridView
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {

                Eskaera.ProduktuakErakutsi(dataGridView1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            mahaiaTextBox.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            dataTextBox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            prezioTotalaTextBox.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
