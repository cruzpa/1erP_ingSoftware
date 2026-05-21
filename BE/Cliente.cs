using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Cliente : Usuario, IObserverCliente
    {
        public override TipoUsuario TipoUsuario => TipoUsuario.CLIENTE;
        public void ActualizarFinSubasta(Subasta subasta)
        {
            Console.WriteLine(
                $"La subasta finalizó. " +
                $"Ganador: {subasta.MejorPostor.Username}. " +
                $"Precio final: {subasta.PrecioFinal}"
            );
        }

        public void ActualizarPrecioSubasta(Subasta subasta)
        {
            Console.WriteLine(
                $"Notificación para {Username}: " +
                $"Nueva oferta en {subasta.Articulo.Nombre}. " +
                $"Precio actual: {subasta.PrecioFinal}"
            );
        }
    }
}
