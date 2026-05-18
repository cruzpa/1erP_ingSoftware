using System;
using System.Collections.Generic;

namespace BE
{
    public class CasaSubasta
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<Subasta> Subastas { get; set; }

        public CasaSubasta()
        {
            Subastas = new List<Subasta>();
        }
        public void AgregarSubasta(Subasta subasta)
        {
            if (!Subastas.Contains(subasta))
            {
                Subastas.Add(subasta);
                Console.WriteLine("Subasta agregada.");
            }
            else
            {
                Console.WriteLine("La subasta ya existe.");
            }
        }

        public void SacarSubasta(Subasta subasta)
        {
            if (Subastas.Contains(subasta))
            {
                Subastas.Remove(subasta);
                Console.WriteLine("Subasta removida.");
            }
            else
            {
                Console.WriteLine("La subasta no existe.");
            }
        }

        public void Ofertar(Subasta subasta, Cliente cliente, decimal monto)
        {
            DateTime now = DateTime.Now;
            if (now < FechaInicio)
            {
                Console.WriteLine("La subasta aún no comenzó.");
                return;
            }

            if (now > FechaFin)
            {
                Console.WriteLine("La subasta ya finalizó.");
                return;
            }

            subasta.Ofertar(cliente, monto);
        }

        public void FinalizarSubastas()
        {
            if (DateTime.Now >= FechaFin)
            {
                foreach (Subasta subasta in Subastas)
                {
                        subasta.Finalizar();
                }
            }
        }
        public void EmitirReporte()
        {
            Console.WriteLine("=== REPORTE DE JORNADA ===");

            foreach (Subasta subasta in Subastas)
            {
                if(subasta.Vendido)
                Console.WriteLine(
                    $"{subasta.Articulo.Nombre} - Precio final: {subasta.PrecioFinal}"
                );

                Console.WriteLine(
                    subasta.Articulo.GetDetalle()
                );

                Console.WriteLine("--------------------------------");
            }
        }
    }
}