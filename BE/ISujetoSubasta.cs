using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface ISujetoSubasta
    {
        void AgregarInteresado(IObserverCliente cliente);
        void SacarInteresado(IObserverCliente cliente);
        void NotificarCambioPrecio();
        void NotificarFinSubasta();
    }
}
