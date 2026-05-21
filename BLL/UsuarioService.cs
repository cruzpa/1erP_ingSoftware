using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class UsuarioService
    {
        private readonly static DAL_Usuario u = new DAL_Usuario();
        public static int Crear (Usuario usuario)
        {
            try
            {
                Usuario usuarioExiste = u.BuscarPorUsername(usuario.Username);
                if (usuarioExiste != null) { throw new Exception("EL USERNAME YA ESTÁ EN USO"); }
                int resultado = u.Crear(usuario);
                if (resultado == 0) throw new Exception("No se creó el usuario");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("CREAR USUARIO - " + ex.Message);
            } 
        }
        public static int CambiarPassword (Usuario usuario)
        {
            try
            {
                int resultado = u.CambiarPassword(usuario);
                if (resultado == 0) throw new Exception("No se cambió el password");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("CAMBIAR PASSWORD - " + ex.Message);
            }
        }
        public static Usuario BuscarPorUsuario (string username)
        {
            try
            {
                return u.BuscarPorUsername(username);
            }
            catch (Exception ex)
            {
                throw new Exception("BUSCAR POR USUARIO - " + ex.Message);
            }
        }
        public static Usuario BuscarPorId(int id)
        {
            try
            {
                return u.BuscarPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("BUSCAR POR Id - " + ex.Message);
            }
        }
        public static List<Usuario> BuscarUsuarios(bool incluireliminados)
        {
            try
            {
                return u.BuscarUsuarios(incluireliminados);
            }
            catch (Exception ex)
            {
                throw new Exception("BUSCAR USUARIOS - " + ex.Message);
            }
        }
        public static int Modificar (Usuario usuario)
        {
            try
            {
                int resultado = u.Modificar(usuario);
                if (resultado == 0) throw new Exception("No se modificó el usuario");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("MODIFICAR USUARIO - " + ex.Message);
            }
        }
        public static int IncrementarIntentosFallidos(Usuario usuario)
        {
            try
            {
                if (usuario.IntentosFallidos > 4)
                {
                    u.Bloquear(usuario);
                }
                usuario.IntentosFallidos++;
                int resultado = u.IncrementarIntentosFallidos(usuario);
                if (resultado == 0) throw new Exception("No se incrementó el contador de intentos fallidos");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("INCREMENTAR INTENTOS FALLIDOS - " + ex.Message);
            }
        }
        public static int ReiniciarIntentosFallidos(Usuario usuario)
        {
            try
            {
                int resultado = u.ReiniciarIntentosFallidos(usuario);
                if (resultado == 0) throw new Exception("No se reinició el contador de intentos fallidos");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("REINICIAR INTENTOS FALLIDOS - " + ex.Message);
            }
        }
        public static int Bloquear(Usuario usuario)
        {
            try
            {
                int resultado = u.Bloquear(usuario);
                if (resultado == 0) throw new Exception("No se bloqueó el usuario");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("BLOQUEAR USUARIO - " + ex.Message);
            }
        }
        public static int Desbloquear(Usuario usuario)
        {
            try
            {
                int resultado = u.Desbloquear(usuario);
                if (resultado == 0) throw new Exception("No se desbloqueó el usuario");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("DESBLOQUEAR USUARIO - " + ex.Message);
            }
        }
        public static int Eliminar(Usuario usuario)
        {
            try
            {
                int resultado = u.Eliminar(usuario);
                if (resultado == 0) throw new Exception("No se eliminó el usuario");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("ELIMINAR USUARIO - " + ex.Message);
            }
        }
        public static int Habilitar(Usuario usuario)
        {
            try
            {
                int resultado = u.Habilitar(usuario);
                if (resultado == 0) throw new Exception("No se habilitó el usuario");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("HABILITAR USUARIO - " + ex.Message);
            }
        }
        public static bool ValidarBloqueado(Usuario usuario)
        {
            if (usuario == null) return false;
            if (usuario.Bloqueado) return false;
            return true;
        }
        public static bool ValidarEliminado(Usuario usuario)
        {
            if (usuario == null) return false;
            if (usuario.Eliminado) return false;
            return true;
        }
    }
}
