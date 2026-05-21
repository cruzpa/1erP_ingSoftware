using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Martillero : Usuario
    {
        public override TipoUsuario TipoUsuario => TipoUsuario.MARTILLERO;
        public override List<Permiso> Permisos => new List<Permiso> { Permiso.ABM_ARTICULOS, Permiso.PARTICIPAR_SUBASTA };

        public string Matricula { get; set; }
    }
}
