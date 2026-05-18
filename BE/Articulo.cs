using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { private get; set; }
        public virtual decimal GetPrecio() { return Precio; }

        public virtual string GetDetalle()
        {
            return $"Artículo: {Nombre} - {Descripcion} - Precio: {GetPrecio()}";
        }
    }
}
