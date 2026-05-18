using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Cliente : IObserverCliente
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public void ActualizarFinSubasta(Subasta subasta)
        {
            Console.WriteLine($"La subasta finalizó. Ganador: {subasta.MejorPostor.Username}. Precio final: {subasta.PrecioFinal}");
        }

        public void ActualizarPrecioSubasta(Subasta subasta)
        {
            Console.WriteLine(
                    $"Notificación para {Username}: " +
                    $"Nueva oferta en {subasta.Articulo.Nombre}. " + 
                    $"Precio actual: {subasta.PrecioFinal}. "
                );
        }


    }

}
