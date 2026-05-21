using BE;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 1 - Login correcto
    /// 2 - Usuario inexistente
    /// 3 - Password incorrecto
    /// 4 - Usuario bloqueado
    /// 5 - Usuario eliminado
    /// 6 - Cambio de password requerido
    /// </summary>
    public static class LoginService
    {
        public static int Login (string username, string password)
        {
            try
            {
                Usuario usuario = UsuarioService.BuscarPorUsuario(username);
                if (usuario == null)
                {
                    BitacoraService.Crear(new BE_Bitacora()
                    {
                        Username = "Sin usuario",
                        FechaYHora = DateTime.UtcNow,
                        Tipo = "WARNING",
                        Descripcion = $"Intento de LOGIN con usuario inexistente ({username})"
                    });
                    return 2;
                }

                if (usuario.Bloqueado)
                {
                    BitacoraService.Crear(new BE_Bitacora()
                    {
                        Username = "Sin usuario",
                        FechaYHora = DateTime.UtcNow,
                        Tipo = "WARNING",
                        Descripcion = $"Intento de LOGIN con usuario bloqueado ({username})"
                    });
                    return 4; 
                }

                if (usuario.Eliminado) 
                {
                    BitacoraService.Crear(new BE_Bitacora()
                    {
                        Username = "Sin usuario",
                        FechaYHora = DateTime.UtcNow,
                        Tipo = "WARNING",
                        Descripcion = $"Intento de LOGIN con usuario eliminado ({username})"
                    });
                    return 5; 
                }

                if (usuario.Password != SeguridadService.Encriptar(password))
                {
                    UsuarioService.IncrementarIntentosFallidos(usuario);
                    BitacoraService.Crear(new BE_Bitacora()
                    {
                        Username = "Sin usuario",
                        FechaYHora = DateTime.UtcNow,
                        Tipo = "WARNING",
                        Descripcion = $"Password incorrecto ({username})"
                    });
                    return 3;
                }
                
                SessionManager.Login(usuario);
                
                if (usuario.IntentosFallidos > 0)
                {
                    UsuarioService.ReiniciarIntentosFallidos(usuario);
                    BitacoraService.Crear(new BE_Bitacora()
                    {
                        Username = SessionManager.GetInstance.usuario.Username,
                        FechaYHora = DateTime.UtcNow,
                        Tipo = "WARNING",
                        Descripcion = $"Reinicio de contador de intentos de inicio de sesión fallidos"
                    });
                }

                BitacoraService.Crear(new BE_Bitacora()
                {
                    Username = SessionManager.GetInstance.usuario.Username,
                    FechaYHora = DateTime.UtcNow,
                    Tipo = "INFO",
                    Descripcion = $"Ingreso al sistema"
                });

                if (password == "cambiar")
                {
                    return 6;
                }
                return 1;
            }
            catch (Exception ex) { throw new Exception("LoginService - " + ex.Message); }
        }
    }
}
