using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using System.Windows.Forms;

namespace TPVApp.Dominio
{
    internal class NH
    {
        private static NHibernate.Cfg.Configuration myConfiguration;
        private static ISessionFactory mySessionFactory;

        public static void InitializeNHibernate()
        {

            try
            {
                myConfiguration = new NHibernate.Cfg.Configuration();
                myConfiguration.Configure(); 
                myConfiguration.AddAssembly(typeof(Eskaera).Assembly);
                myConfiguration.AddAssembly(typeof(ProduktuEskaera).Assembly);  

                mySessionFactory = myConfiguration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar NHibernate: " + ex.Message);
            }
        }
    }
}
