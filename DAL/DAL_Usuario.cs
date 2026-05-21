using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class DAL_Usuario
    {
        private readonly Acceso acceso = new Acceso();

        private Usuario MapearUsuario(SqlDataReader reader)
        {
            Usuario usuario;

            string tipoUsuario = reader["TipoUsuario"].ToString();

            switch (tipoUsuario)
            {
                case "CLIENTE":
                    usuario = new Cliente();
                    break;

                case "MARTILLERO":
                    usuario = new Martillero();
                    break;

                default:
                    throw new Exception("Tipo de usuario inválido");
            }

            usuario.Id = int.Parse(reader["Id"].ToString());
            usuario.Username = reader["Username"].ToString();
            usuario.Password = reader["Password"].ToString();
            usuario.Nombre = reader["Nombre"].ToString();
            usuario.Apellido = reader["Apellido"].ToString();
            usuario.Email = reader["Email"].ToString();
            usuario.Telefono = reader["Telefono"].ToString();
            usuario.Direccion = reader["Direccion"].ToString();

            usuario.IntentosFallidos = int.Parse(reader["IntentosFallidos"].ToString());

            usuario.Bloqueado = bool.Parse(reader["Bloqueado"].ToString());
            usuario.Eliminado = bool.Parse(reader["Eliminado"].ToString());

            return usuario;
        }
        public int Crear(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Username", usuario.Username),
                    acceso.CrearParametro("@Password", usuario.Password),
                    acceso.CrearParametro("@Nombre", usuario.Nombre),
                    acceso.CrearParametro("@Apellido", usuario.Apellido),
                    acceso.CrearParametro("@Email", usuario.Email),
                    acceso.CrearParametro("@Telefono", usuario.Telefono),
                    acceso.CrearParametro("@Direccion", usuario.Direccion),
                    acceso.CrearParametro("@TipoUsuario", usuario.TipoUsuario.ToString())
                };

                resultado = acceso.Escribir(
                    @"insert into Usuario
                    (
                        Username,
                        Password,
                        Nombre,
                        Apellido,
                        Email,
                        Telefono,
                        Direccion,
                        IntentosFallidos,
                        Bloqueado,
                        Eliminado,
                        TipoUsuario
                    )
                    values
                    (
                        @Username,
                        @Password,
                        @Nombre,
                        @Apellido,
                        @Email,
                        @Telefono,
                        @Direccion,
                        0,
                        0,
                        0,
                        @TipoUsuario
                    )",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-CREAR USUARIO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
        public int CambiarPassword(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id),
                    acceso.CrearParametro("@Password", usuario.Password)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.Password = @Password where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-CAMBIAR PASSWORD - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }


        public Usuario BuscarPorUsername(string username)
        {
            if (username == string.Empty) return null;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Username", username)
                };

                SqlDataReader reader = acceso.Leer(
                    @"select
                        Usuario.Id,
                        Usuario.Username,
                        Usuario.Password,
                        Usuario.Nombre,
                        Usuario.Apellido,
                        Usuario.Email,
                        Usuario.Telefono,
                        Usuario.Direccion,
                        Usuario.IntentosFallidos,
                        Usuario.Bloqueado,
                        Usuario.Eliminado,
                        Usuario.TipoUsuario
                    from Usuario
                    where Usuario.Username = @Username",
                    parametros
                );

                if (reader.Read())
                {
                    return MapearUsuario(reader);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-BUSCAR USUARIO POR USERNAME - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public Usuario BuscarPorId(int id)
        {
            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", id)
                };

                SqlDataReader reader = acceso.Leer(
                    @"select
                        Usuario.Id,
                        Usuario.Username,
                        Usuario.Password,
                        Usuario.Nombre,
                        Usuario.Apellido,
                        Usuario.Email,
                        Usuario.Telefono,
                        Usuario.Direccion,
                        Usuario.IntentosFallidos,
                        Usuario.Bloqueado,
                        Usuario.Eliminado,
                        Usuario.TipoUsuario
                    from Usuario
                    where Usuario.Id = @Id",
                    parametros
                );

                if (reader.Read())
                {
                    return MapearUsuario(reader);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-BUSCAR USUARIO POR ID - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }
        }
        public List<Usuario> BuscarUsuarios(bool incluirEliminados)
        {
            acceso.Abrir();

            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                SqlDataReader reader;

                if (incluirEliminados)
                {
                    reader = acceso.Leer(
                        @"select
                            Usuario.Id,
                            Usuario.Username,
                            Usuario.Password,
                            Usuario.Nombre,
                            Usuario.Apellido,
                            Usuario.Email,
                            Usuario.Telefono,
                            Usuario.Direccion,
                            Usuario.IntentosFallidos,
                            Usuario.Bloqueado,
                            Usuario.Eliminado,
                            Usuario.TipoUsuario
                        from Usuario"
                    );
                }
                else
                {
                    reader = acceso.Leer(
                        @"select
                            Usuario.Id,
                            Usuario.Username,
                            Usuario.Password,
                            Usuario.Nombre,
                            Usuario.Apellido,
                            Usuario.Email,
                            Usuario.Telefono,
                            Usuario.Direccion,
                            Usuario.IntentosFallidos,
                            Usuario.Bloqueado,
                            Usuario.Eliminado,
                            Usuario.TipoUsuario
                        from Usuario
                        where Usuario.Eliminado = 0"
                    );
                }

                while (reader.Read())
                {
                    usuarios.Add(MapearUsuario(reader));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-BUSCAR USUARIOS - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return usuarios;
        }

        public int Modificar(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id),
                    acceso.CrearParametro("@Nombre", usuario.Nombre),
                    acceso.CrearParametro("@Apellido", usuario.Apellido),
                    acceso.CrearParametro("@Email", usuario.Email),
                    acceso.CrearParametro("@Telefono", usuario.Telefono),
                    acceso.CrearParametro("@Direccion", usuario.Direccion)
                };

                resultado = acceso.Escribir(
                    @"update Usuario
                    set
                        Usuario.Nombre = @Nombre,
                        Usuario.Apellido = @Apellido,
                        Usuario.Email = @Email,
                        Usuario.Telefono = @Telefono,
                        Usuario.Direccion = @Direccion
                    where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-MODIFICAR USUARIO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public int IncrementarIntentosFallidos(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id),
                    acceso.CrearParametro("@IntentosFallidos", usuario.IntentosFallidos)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.IntentosFallidos = @IntentosFallidos where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-INCREMENTAR INTENTOS FALLIDOS - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
        public int ReiniciarIntentosFallidos(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.IntentosFallidos = 0 where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-REINICIAR INTENTOS FALLIDOS - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
        public int Bloquear(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.Bloqueado = 1 where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-BLOQUEAR USUARIO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
        public int Desbloquear(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.Bloqueado = 0 where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-DESBLOQUEAR USUARIO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
        public int Eliminar(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.Eliminado = 1 where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-ELIMINAR USUARIO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
        public int Habilitar(Usuario usuario)
        {
            int resultado = 0;

            if (usuario == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", usuario.Id)
                };

                resultado = acceso.Escribir(
                    "update Usuario set Usuario.Eliminado = 0 where Usuario.Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-HABILITAR USUARIO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }
    }
}
