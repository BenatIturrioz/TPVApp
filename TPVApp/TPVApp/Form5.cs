using System;
using System.Windows.Forms;
using TPVApp.Dominio;

namespace TPVApp
{
    public partial class Form5 : Form
    {
        public Eskaera eskaera;

        public Form5(Eskaera eskaera)
        {
            InitializeComponent();
            this.eskaera = eskaera;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // Puedes agregar lógica adicional al cargar Form5 si es necesario.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(eskaera, 1);
            form6.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(eskaera, 2);
            form6.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(eskaera, 4);
            form6.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(eskaera, 3);
            form6.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Eskaera.gordeEskaera(eskaera);
            Form2 form2 = new Form2(eskaera.Langilea_id);
            form2.Show();
            this.Close();
        }
    }
}
