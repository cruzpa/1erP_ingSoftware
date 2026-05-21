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
        public EstadoArticulo Estado { get; set; }
        private decimal precio;
        public virtual decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }
        public virtual string Tipo => "Articulo";

        public Articulo()
        {
            Estado = EstadoArticulo.Disponible;
        }

        public override string ToString()
        {
            return $"Artículo: {Nombre} - {Descripcion} - Precio: {Precio}";
        }
    }
}
