using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NHibernate;

namespace TPVApp.Dominio
{
    public class Eskaera
    {
        public virtual int Id { get; set; }
        public virtual int Langilea_id { get; set; }
        public virtual int Erreserba_id { get; set; }
        public virtual int Mahaia { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual float PrezioTotala { get; set; }
        public virtual bool Ordaindua { get; set; }

        public static void EskaerakErakutsi(DataGridView dataGridView1)
        {
            try
            {
                using (var session = NH.OpenSession())
                {
                    Console.WriteLine("Sesión de NHibernate abierta.");

                    var eskaerak = session.Query<Eskaera>()
                                           .Where(e => e.Ordaindua != true) 
                                           .ToList();
                    Console.WriteLine($"Datos recuperados: {eskaerak.Count}");

                    var tabla1 = new DataTable();
                    tabla1.Columns.Add("Erreserba ID", typeof(int));
                    tabla1.Columns.Add("Mahaia ID", typeof(int));
                    tabla1.Columns.Add("Data", typeof(DateTime));
                    tabla1.Columns.Add("Prezio Totala", typeof(float));

                    foreach (var eskaera in eskaerak)
                    {
                        tabla1.Rows.Add(eskaera.Erreserba_id, eskaera.Mahaia, eskaera.Data, eskaera.PrezioTotala);
                    }

                    dataGridView1.DataSource = tabla1;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    Console.WriteLine("Datos asignados al DataGridView.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
                throw; 
            }
        }


        internal static void gordeEskaera(Eskaera eskaera)
        {
            using (ISession session = NH.OpenSession())  
            {
                var listaDeProductos = session.QueryOver<ProduktuEskaera>()
                                              .Where(pe => pe.ErreserbaId == eskaera.Erreserba_id)
                                              .List<ProduktuEskaera>();

                float totalPrecio = listaDeProductos.Sum(pe => pe.Prezioa * pe.ProduktuaKop);  // Multiplicamos por la cantidad de cada producto

                eskaera.PrezioTotala = totalPrecio;
                eskaera.Data = DateTime.Now;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(eskaera); 

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al guardar la Eskaera con el precio total y la fecha/hora actual", ex);
                    }
                }
            }
        }


    }
}
