using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE;
using DAL;

namespace BLL
{
    public class CasaSubastaService
    {
        private static readonly object bloqueoInstancia = new object();
        private static CasaSubastaService instancia;
        private readonly MapperSubasta mapperSubasta = new MapperSubasta();

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

            if (CargarJornadaActiva())
            {
                return;
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

                if (articulo.Estado != EstadoArticulo.Disponible)
                {
                    continue;
                }

                AgregarSubasta(new Subasta(articulo, CasaSubasta.FechaInicio, CasaSubasta.FechaFin));
                ActualizarEstadoUnidadVenta(articulo, EstadoArticulo.EnSubasta);
            }

            mapperSubasta.GuardarJornada(CasaSubasta.Subastas);
        }

        public bool CargarJornadaActiva()
        {
            List<Subasta> subastasActivas = mapperSubasta.ListarVigentes();

            if (subastasActivas.Count == 0)
            {
                return false;
            }

            CasaSubasta.Subastas = subastasActivas;
            CasaSubasta.FechaInicio = subastasActivas.First().FechaInicio;
            CasaSubasta.FechaFin = subastasActivas.First().FechaFin;

            if (DateTime.Now >= CasaSubasta.FechaFin)
            {
                FinalizarSubastas();
                return false;
            }

            return true;
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
            mapperSubasta.Editar(subasta);
        }

        public void FinalizarSubastas(bool forzar = false)
        {
            if (!forzar && DateTime.Now < CasaSubasta.FechaFin)
            {
                return;
            }

            foreach (Subasta subasta in CasaSubasta.Subastas)
            {
                if (subasta.Estado == EstadoSubasta.Activa)
                {
                    subasta.Finalizar();
                    mapperSubasta.Editar(subasta);

                    EstadoArticulo estadoArticulo = subasta.MejorPostor != null
                        ? EstadoArticulo.Vendido
                        : EstadoArticulo.Disponible;

                    ActualizarEstadoUnidadVenta(subasta.Articulo, estadoArticulo);
                }
            }

            CasaSubasta.Subastas = CasaSubasta.Subastas
                .Where(s => s.Estado == EstadoSubasta.Activa)
                .ToList();
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

        private void ActualizarEstadoUnidadVenta(Articulo articulo, EstadoArticulo estado)
        {
            if (articulo == null)
            {
                return;
            }

            articulo.Estado = estado;
            ArticuloService.ActualizarEstado(articulo.Id, estado);

            Lote lote = articulo as Lote;

            if (lote == null)
            {
                return;
            }

            foreach (Articulo articuloLote in lote.Articulos)
            {
                articuloLote.Estado = estado;
                ArticuloService.ActualizarEstado(articuloLote.Id, estado);
            }
        }
    }
}
