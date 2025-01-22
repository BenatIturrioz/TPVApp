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
    public partial class Form4 : Form
    {
        // Declarar la propiedad para almacenar el objeto Eskaera
        private Eskaera eskaera;

        // Constructor que recibe el objeto Eskaera
        public Form4(Eskaera eskaera)
        {
            InitializeComponent();
            this.eskaera = eskaera; // Asignar el objeto recibido a la propiedad
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // Llamar al método MahaiakErakutsi pasando el objeto eskaera
            Mahaia.MahaiakErakutsi(eskaera, flowLayoutPanel1, this);
        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }

}

