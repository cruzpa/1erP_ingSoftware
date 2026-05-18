using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Subasta : ISujetoSubasta
    {
        public int Id { get; set; }
        public bool Vendido { get; set; }
        public Cliente MejorPostor { get; set; }
        public string Descripcion { get; set; }
        public bool Activa { get; set; }

        private decimal precioFinal;

        public decimal PrecioFinal
        {
            get { return precioFinal; }
            set
            {
                precioFinal = value;
                this.NotificarCambioPrecio();
            }
        }
        public Articulo Articulo { get; set; }
        public List<IObserverCliente> Interesados { get; set; }

        public Subasta()
        {
            Interesados = new List<IObserverCliente>();
        }

        public void AgregarInteresado(IObserverCliente IOcliente)
        {
            if (!Interesados.Contains(IOcliente))
            {
                Interesados.Add(IOcliente);
                Console.WriteLine("El cliente ahora está interesado en esta subasta.");
            }
            else
            {
                Console.WriteLine("El cliente ya está interesado en esta subasta.");
            }
        }

        public void SacarInteresado(IObserverCliente IOcliente)
        {
            if (Interesados.Contains(IOcliente))
            {
                Interesados.Remove(IOcliente);
                Console.WriteLine("El cliente ya no esta interesado en esta subasta.");

            }
            else
            {
                Console.WriteLine("El cliente no estaba interesado en esta subasta.");
            }
        }

        public void NotificarCambioPrecio()
        {
            if(Interesados.Count == 0)
            {
                Console.WriteLine("No hay clientes interesados en esta subasta.");
                return;
            }
            Console.WriteLine("Notificando a los interesados..");
            foreach (IObserverCliente cliente in Interesados)
            {
                cliente.ActualizarPrecioSubasta(this);
            }
        }

        public void NotificarFinSubasta()
        {
            if (Interesados.Count == 0)
            {
                Console.WriteLine("No hay clientes interesados en esta subasta.");
                return;
            }
            Console.WriteLine("Notificando a los interesados..");
            foreach (IObserverCliente cliente in Interesados)
            {
                cliente.ActualizarFinSubasta(this);
            }
        }

        public void Ofertar(Cliente cliente, decimal monto)
        {
            if (!Activa)
            {
                Console.WriteLine("La subasta no esta activa.");
                return;
            }

            if (Vendido)
            {
                Console.WriteLine("El producto ya fue vendido.");
                return;
            }

            if (monto > PrecioFinal)
            {
                PrecioFinal = monto;
                MejorPostor = cliente;

                Console.WriteLine($"{cliente.Username} realizó una oferta de {monto}");
            }
        }

        public void Finalizar()
        {
            if (Vendido)
            {
                //throw new InvalidOperationException("La subasta ya ha sido finalizada.");
                Console.WriteLine("La subasta ya ha sido finalizada.");
            }

            Vendido = true;

            string ganador = MejorPostor != null
                ? MejorPostor.Username
                : "Sin ofertas";

            NotificarFinSubasta();
        }

    }
}
