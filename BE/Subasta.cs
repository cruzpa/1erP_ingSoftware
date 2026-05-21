using System;
using System.Collections.Generic;

namespace BE
{
    public class Subasta : ISujetoSubasta
    {
        private readonly object bloqueoOferta = new object();

        public int Id { get; set; }
        public Articulo Articulo { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public decimal PrecioInicial { get; set; }

        private decimal precioFinal;
        public decimal PrecioFinal
        {
            get { return precioFinal; }
            private set
            {
                precioFinal = value;
                NotificarCambioPrecio();
            }
        }

        public Cliente MejorPostor { get; private set; }
        public EstadoSubasta Estado { get; private set; }

        public bool Activa => Estado == EstadoSubasta.Activa;
        public bool Vendido => Estado == EstadoSubasta.Finalizada && MejorPostor != null;

        public List<IObserverCliente> Interesados { get; set; }

        public Subasta()
        {
            Interesados = new List<IObserverCliente>();
            Estado = EstadoSubasta.Activa;
        }

        public Subasta(Articulo articulo, DateTime fechaInicio, DateTime fechaFin) : this()
        {
            if (articulo == null)
            {
                throw new ArgumentNullException("articulo");
            }

            if (fechaFin <= fechaInicio)
            {
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            Id = articulo.Id;
            Articulo = articulo;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            PrecioInicial = articulo.Precio;
            precioFinal = articulo.Precio;
        }

        public void AgregarInteresado(IObserverCliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException("cliente");
            }

            if (!Interesados.Contains(cliente))
            {
                Interesados.Add(cliente);
            }
        }

        public void SacarInteresado(IObserverCliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException("cliente");
            }

            Interesados.Remove(cliente);
        }

        public void Ofertar(Cliente cliente, decimal monto)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException("cliente");
            }

            lock (bloqueoOferta)
            {
                if (Estado != EstadoSubasta.Activa)
                {
                    throw new InvalidOperationException("La subasta no esta activa.");
                }

                DateTime now = DateTime.Now;

                if (now < FechaInicio)
                {
                    throw new InvalidOperationException("La subasta aun no comenzo.");
                }

                if (now > FechaFin)
                {
                    throw new InvalidOperationException("La subasta ya finalizo.");
                }

                if (monto <= PrecioFinal)
                {
                    throw new InvalidOperationException("La oferta debe ser superior al precio actual.");
                }

                MejorPostor = cliente;
                PrecioFinal = monto;
            }
        }

        public void Finalizar()
        {
            if (Estado == EstadoSubasta.Finalizada)
            {
                throw new InvalidOperationException("La subasta ya ha sido finalizada.");
            }

            Estado = EstadoSubasta.Finalizada;
            NotificarFinSubasta();
        }

        public void Cancelar()
        {
            if (Estado == EstadoSubasta.Finalizada)
            {
                throw new InvalidOperationException("No se puede cancelar una subasta finalizada.");
            }

            Estado = EstadoSubasta.Cancelada;
        }

        public void NotificarCambioPrecio()
        {
            foreach (IObserverCliente cliente in Interesados)
            {
                cliente.ActualizarPrecioSubasta(this);
            }
        }

        public void NotificarFinSubasta()
        {
            foreach (IObserverCliente cliente in Interesados)
            {
                cliente.ActualizarFinSubasta(this);
            }
        }
    }
}