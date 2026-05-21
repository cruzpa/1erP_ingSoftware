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
        public override string Tipo => "Lote";

        public Lote()
        {
            Articulos = new List<Articulo>();
        }

        public override decimal Precio
        {
            get
            {
                return Articulos.Sum(a => a.Precio);
            }

            set { }
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
        public override string ToString()
        {
            return $"Lote: {Nombre} - " +
                   $"Descripción: {Descripcion} - " +
                   $"Contenido: {string.Join(", ", Articulos)} - " +
                   $"Precio total: {Precio}";
        }
    }
}
