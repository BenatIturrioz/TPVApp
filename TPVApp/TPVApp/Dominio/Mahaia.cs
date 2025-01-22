using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Linq;

namespace TPVApp.Dominio
{
    public class Mahaia
    {
        public virtual int Mahaia_id { get; set; }
        public virtual int Zenbakia { get; set; }
        public virtual int BezeroKopMax { get; set; }

        internal static void MahaiakErakutsi(Eskaera eskaera, FlowLayoutPanel flowLayoutPanel, Form4 form4)
        {
            // Limpiar el FlowLayoutPanel antes de añadir botones
            flowLayoutPanel.Controls.Clear();

            try
            {
                using (ISession session = NH.OpenSession())
                {
                    // Consultar la lista de Mahaiak
                    var mahaiak = session.Query<Mahaia>().ToList();

                    foreach (var mahaia in mahaiak)
                    {
                        // Verificar si existe una Eskaera no pagada para esta mesa
                        bool hasUnpaidOrder = session.Query<Eskaera>()
                                                     .Any(e => e.Mahaia == mahaia.Mahaia_id && e.Ordaindua == false);

                        // Crear un botón dinámico para cada Mahaia
                        Button button = new Button
                        {
                            Text = $"Mahaia {mahaia.Zenbakia}",
                            Width = 100,
                            Height = 60,
                            Tag = mahaia.Mahaia_id,
                            Enabled = !hasUnpaidOrder // Deshabilitar el botón si hay una Eskaera no pagada
                        };

                        if (hasUnpaidOrder)
                        {
                            button.BackColor = Color.Red; // Opcional: Cambiar color si la mesa está ocupada
                        }
                        else
                        {
                            button.BackColor = Color.Green; // Opcional: Cambiar color si la mesa está disponible
                        }

                        button.Click += (sender, e) =>
                        {
                            int selectedMahaiaId = (int)((Button)sender).Tag;
                            eskaera.Mahaia = selectedMahaiaId;
                            Form5 form5 = new Form5(eskaera);
                            form5.Show();
                            form4.Close();
                        };

                        flowLayoutPanel.Controls.Add(button);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los mahaiak: {ex.Message}");
            }
        }

    }
}
