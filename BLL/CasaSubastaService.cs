using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE;

namespace BLL
{
    public class CasaSubastaService
    {
        private static readonly object bloqueoInstancia = new object();
        private static CasaSubastaService instancia;

        public static CasaSubastaService GetInstance
        {
            get
            {
                lock (bloqueoInstancia)
                {
                    if (instancia == null)
                    {
                        instancia = new CasaSubastaService();
                    }

                    return instancia;
                }
            }
        }

        public CasaSubasta CasaSubasta { get; private set; }

        private CasaSubastaService()
        {
            CasaSubasta = new CasaSubasta();
        }

        public void ConfigurarJornada(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaFin <= fechaInicio)
            {
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            CasaSubasta.FechaInicio = fechaInicio;
            CasaSubasta.FechaFin = fechaFin;
        }

        public void CrearSubastasDesdeCatalogo(List<Articulo> articulos)
        {
            if (articulos == null)
            {
                throw new ArgumentNullException("articulos");
            }

            HashSet<int> articulosDentroDeLotes = new HashSet<int>();

            foreach (Lote lote in articulos.OfType<Lote>())
            {
                foreach (Articulo articulo in lote.Articulos)
                {
                    articulosDentroDeLotes.Add(articulo.Id);
                }
            }

            CasaSubasta.Subastas.Clear();

            foreach (Articulo articulo in articulos)
            {
                if (!(articulo is Lote) && articulosDentroDeLotes.Contains(articulo.Id))
                {
                    continue;
                }

                AgregarSubasta(new Subasta(articulo, CasaSubasta.FechaInicio, CasaSubasta.FechaFin));
            }
        }

        public List<Subasta> ListarSubastas()
        {
            return CasaSubasta.Subastas;
        }

        public void AgregarSubasta(Subasta subasta)
        {
            if (subasta == null)
            {
                throw new ArgumentNullException("subasta");
            }

            bool existe = CasaSubasta.Subastas.Any(s => s.Articulo != null && subasta.Articulo != null && s.Articulo.Id == subasta.Articulo.Id);

            if (!existe)
            {
                CasaSubasta.Subastas.Add(subasta);
            }
        }

        public void SacarSubasta(Subasta subasta)
        {
            if (subasta == null)
            {
                throw new ArgumentNullException("subasta");
            }

            CasaSubasta.Subastas.Remove(subasta);
        }

        public void Suscribir(Subasta subasta, Cliente cliente)
        {
            if (subasta == null)
            {
                throw new ArgumentNullException("subasta");
            }

            subasta.AgregarInteresado(cliente);
        }

        public void Desuscribir(Subasta subasta, Cliente cliente)
        {
            if (subasta == null)
            {
                throw new ArgumentNullException("subasta");
            }

            subasta.SacarInteresado(cliente);
        }

        public void Ofertar(Subasta subasta, Cliente cliente, decimal monto)
        {
            if (subasta == null)
            {
                throw new ArgumentNullException("subasta");
            }

            DateTime now = DateTime.Now;

            if (now < CasaSubasta.FechaInicio)
            {
                throw new InvalidOperationException("La subasta aun no comenzo.");
            }

            if (now > CasaSubasta.FechaFin)
            {
                throw new InvalidOperationException("La subasta ya finalizo.");
            }

            subasta.Ofertar(cliente, monto);
        }

        public void FinalizarSubastas()
        {
            if (DateTime.Now < CasaSubasta.FechaFin)
            {
                return;
            }

            foreach (Subasta subasta in CasaSubasta.Subastas)
            {
                if (!subasta.Vendido)
                {
                    subasta.Finalizar();
                }
            }
        }

        public List<string> GenerarReporteConsolidado()
        {
            List<string> reporte = new List<string>();

            foreach (Subasta subasta in CasaSubasta.Subastas)
            {
                string ganador = subasta.MejorPostor != null
                    ? subasta.MejorPostor.Username
                    : "Sin ofertas";

                reporte.Add(
                    $"{subasta.Articulo.Tipo}: {subasta.Articulo.Nombre} - Precio final: {subasta.PrecioFinal} - Ganador: {ganador}"
                );
            }

            return reporte;
        }

        public string EmitirReporte()
        {
            StringBuilder reporte = new StringBuilder();
            reporte.AppendLine("=== REPORTE DE JORNADA ===");

            foreach (string linea in GenerarReporteConsolidado())
            {
                reporte.AppendLine(linea);
                reporte.AppendLine("--------------------------------");
            }

            return reporte.ToString();
        }
    }
}
