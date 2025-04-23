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
        // Eskaera objektua gordetzeko propietatea
        private Eskaera eskaera;

        // Eraikitzailea: Eskaera objektua jasotzen du
        public Form4(Eskaera eskaera)
        {
            InitializeComponent();
            this.eskaera = eskaera; // Jasotako objektua esleitu
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // Mahaia erakusteko metodoa deitzen da, eskaera objektua pasatuz
            Mahaia.MahaiakErakutsi(eskaera, flowLayoutPanel1, this);
        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Hemen ez dago kode funtzionalik oraindik
        }
    }
}
