using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IObserverCliente
    {
        void ActualizarPrecioSubasta(Subasta subasta);
        void ActualizarFinSubasta(Subasta subasta);
    }
}
