using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using TPVApp.Dominio;

namespace TPVApp.Dominio
{
    public class Produktua
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }  // Cambié el tipo de int a string para representar el nombre del producto
        public virtual float ErosketaPrezioa { get; set; }
        public virtual int Kantitatea { get; set; }
        public virtual int Mota { get; set; }

        public static List<Produktua> ProduktuaErakutsi(int mota)
        {
            // Crear una instancia de la sesión de NHibernate
            using (ISession session = NH.OpenSession())
            {
                // Realizar la consulta para obtener los productos filtrados por "mota"
                var productos = session.QueryOver<Produktua>()
                                       .Where(p => p.Mota == mota)
                                       .List<Produktua>();  // Usamos List<Produktua> para obtener los resultados

                return (List<Produktua>)productos;
            }
        }
    }
}
