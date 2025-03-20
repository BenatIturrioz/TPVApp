using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TPVApp.Dominio;

namespace TPVApp.Dominio
{
    internal class ProduktuEskaera
    {
        public virtual int Id { get; set; }
        public virtual int ErreserbaId { get; set; }
        public virtual string Produktu_izena { get; set; }
        public virtual int ProduktuaKop { get; set; }
        public virtual float Prezioa { get; set; }

        // Método para obtener los productos de la "eskaera" con NHibernate
        public static void ProduktuEskaerakErakutsi(DataGridView dataGridView1, int erreserba_id)
        {
            // Crear una instancia de la sesión de NHibernate
            using (ISession session = NH.OpenSession())
            {
                // Realizamos la consulta usando NHibernate
                var eskaeraProductos = session.QueryOver<ProduktuEskaera>()
                                              .Where(pe => pe.ErreserbaId == erreserba_id)
                                              .List<ProduktuEskaera>();

                // Convertimos la lista de objetos a un DataTable para mostrar en el DataGridView
                DataTable tabla1 = ConvertToDataTable(eskaeraProductos);

                // Establecer el DataSource del DataGridView
                dataGridView1.DataSource = tabla1;

                // Ajustar las columnas y filas del DataGridView
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }

        // Método auxiliar para convertir una lista de objetos en un DataTable
        private static DataTable ConvertToDataTable(IEnumerable<ProduktuEskaera> list)
        {
            DataTable table = new DataTable();

            // Obtener las propiedades de la clase "ProduktuEskaera" y agregarlas como columnas
            var properties = typeof(ProduktuEskaera).GetProperties();
            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            // Rellenar el DataTable con los datos de la lista
            foreach (var item in list)
            {
                DataRow row = table.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(item, null);
                }
                table.Rows.Add(row);
            }

            return table;
        }

        // Método para obtener el siguiente ErreserbaId disponible en la base de datos
        public static int SiguienteErreserbaId()
        {
            using (ISession session = NH.OpenSession())
            {
                // Realizar una consulta para obtener el máximo ErreserbaId actual
                int? maxErreserbaId = session.QueryOver<ProduktuEskaera>()
                                             .Select(Projections.Max<ProduktuEskaera>(pe => pe.ErreserbaId))
                                             .SingleOrDefault<int?>();

                // Si no hay registros, el siguiente ErreserbaId será 1
                return maxErreserbaId.HasValue ? maxErreserbaId.Value + 1 : 1;
            }
        }


        public static void gordeProduktuEskaera(Eskaera eskaera, string nombre, int cantidadElegida, float prezioa)
        {
            using (ISession session = NH.OpenSession()) // Inicia sesión con NHibernate
            {
                using (ITransaction transaction = session.BeginTransaction()) // Iniciar una transacción
                {
                    try
                    {
                        // Buscar el producto correspondiente
                        Produktua produktua = session.Query<Produktua>()
                            .FirstOrDefault(p => p.Izena == nombre);

                        if (produktua == null)
                        {
                            throw new Exception($"El producto con nombre '{nombre}' no existe.");
                        }

                        // Verificar si ya existe un registro con el mismo `ErreserbaId` y `Produktua_izena`
                        ProduktuEskaera produktuEskaera = session.Query<ProduktuEskaera>()
                            .FirstOrDefault(pe => pe.ErreserbaId == eskaera.Erreserba_id && pe.Produktu_izena == nombre);

                        if (produktuEskaera != null)
                        {
                            // Revertir la cantidad previamente seleccionada
                            produktua.Kantitatea += produktuEskaera.ProduktuaKop;

                            if (cantidadElegida == 0)
                            {
                                // Si la nueva cantidad seleccionada es 0, eliminar el registro existente
                                session.Delete(produktuEskaera);
                            }
                            else
                            {
                                // Actualizar el registro existente con la nueva cantidad
                                produktuEskaera.ProduktuaKop = cantidadElegida;
                                produktuEskaera.Prezioa = prezioa;

                                // Restar la nueva cantidad seleccionada
                                produktua.Kantitatea -= cantidadElegida;
                                session.Update(produktuEskaera);
                            }
                        }
                        else
                        {
                            if (cantidadElegida > 0)
                            {
                                // Si no existe un registro previo, crear uno nuevo solo si cantidadElegida > 0
                                produktuEskaera = new ProduktuEskaera
                                {
                                    ErreserbaId = eskaera.Erreserba_id,
                                    Produktu_izena = nombre,
                                    ProduktuaKop = cantidadElegida,
                                    Prezioa = prezioa
                                };

                                produktua.Kantitatea -= cantidadElegida;
                                session.Save(produktuEskaera);
                            }
                        }

                        // Verificar que la cantidad disponible no sea negativa
                        if (produktua.Kantitatea < 0)
                        {
                            throw new Exception($"La cantidad disponible no puede ser negativa para el producto '{nombre}'.");
                        }

                        // Actualizar el producto con la nueva cantidad disponible
                        session.Update(produktua);

                        transaction.Commit(); // Confirmar la transacción si todo va bien
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Revertir la transacción en caso de error
                        throw new Exception("Error al guardar o actualizar la ProduktuEskaera", ex);
                    }
                }
            }
        }





    }
}
