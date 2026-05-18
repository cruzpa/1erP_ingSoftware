using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Lote : Articulo
    {
        public List<Articulo> Articulos { get; set; }
        public override decimal GetPrecio() {
            decimal totalPrice = 0;
            foreach (Articulo articulo in Articulos)
            {
                totalPrice += articulo.GetPrecio();
            }
            return totalPrice;
        }

        public void AgregarArticulo(Articulo articulo)
        {
            if (!Articulos.Contains(articulo))
            {
                Articulos.Add(articulo);
                Console.WriteLine("Artículo agregado al lote.");
            }
            else
            {
                Console.WriteLine("El artículo ya está en el lote.");
            }
        }
        public void SacarArticulo(Articulo articulo)
        {
            if (Articulos.Contains(articulo))
            {
                Articulos.Remove(articulo);
                Console.WriteLine("Artículo removido del lote.");
            }
            else
            {
                Console.WriteLine("El artículo no está en el lote.");
            }
        }
        public override string GetDetalle()
        {
            string detalle = $"Lote: {Nombre}\n";
            detalle += $"Descripción: {Descripcion}\n";
            detalle += $"Contenido:\n";

            foreach (Articulo articulo in Articulos)
            {
                detalle += $"- {articulo.GetDetalle()}\n";
            }

            detalle += $"Precio total: {GetPrecio()}";

            return detalle;
        }
    }
}
