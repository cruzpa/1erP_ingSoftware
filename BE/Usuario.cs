using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Password { get; set; }

        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
        public bool Eliminado { get; set; }
        public abstract TipoUsuario TipoUsuario { get; }
        public abstract List<Permiso> Permisos { get; }

        public bool TienePermiso(Permiso permiso)
        {
            return Permisos.Contains(permiso);
        }
    }
}
