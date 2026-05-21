using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_Bitacora
    {
        private readonly Acceso acceso = new Acceso();
        public int Crear(BE_Bitacora bitacora)
        {
            int resultado = 0;
            if (bitacora == null) return resultado;
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Username", bitacora.Username),
                    acceso.CrearParametro("@FechaYHora", bitacora.FechaYHora.ToString()),
                    acceso.CrearParametro("@Tipo", bitacora.Tipo),
                    acceso.CrearParametro("@Descripcion", bitacora.Descripcion)
                };
                resultado = acceso.Escribir($"insert into Bitacora (Username, FechaYHora, Tipo, Descripcion) values (@Username, @FechaYHora, @Tipo, @Descripcion)", parametros);
            }
            catch (Exception ex) { throw new Exception("DAL-CREAR BITACORA - " + ex.Message); }
            finally { acceso.Cerrar(); }
            return resultado;
        }
        public List<BE_Bitacora> Buscar(DateTime fechaInicio, DateTime fechaFin)
        {
            acceso.Abrir();
            List<BE_Bitacora> lista = new List<BE_Bitacora>();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@FechaInicio", fechaInicio.ToString()),
                    acceso.CrearParametro("@FechaFin", fechaFin.ToString())
                };

                SqlDataReader reader = acceso.Leer("select Id, Username, FechaYHora, Tipo, Descripcion from Bitacora where FechaYHora >= @FechaInicio and FechaYHora <= @FechaFin", parametros);
                while (reader.Read())
                {
                    BE_Bitacora bitacora = new BE_Bitacora();
                    bitacora.Id = int.Parse(reader["Id"].ToString());
                    bitacora.Username = reader["Username"].ToString();
                    bitacora.FechaYHora = DateTime.Parse(reader["FechaYHora"].ToString());
                    bitacora.Tipo = reader["Tipo"].ToString();
                    bitacora.Descripcion = reader["Descripcion"].ToString();
                    lista.Add(bitacora);
                }
            }
            catch (Exception ex) { throw new Exception("DAL-BUSCAR BITACORA POR FECHAS - " + ex.Message); }
            finally { acceso.Cerrar(); }
            return lista;
        }
        public List<BE_Bitacora> Buscar(string tipo, DateTime fechaInicio, DateTime fechaFin)
        {
            acceso.Abrir();
            List<BE_Bitacora> lista = new List<BE_Bitacora>();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Tipo", tipo),
                    acceso.CrearParametro("@FechaInicio", fechaInicio.ToString()),
                    acceso.CrearParametro("@FechaFin", fechaFin.ToString())
                };
                SqlDataReader reader = acceso.Leer("select Id, Username, FechaYHora, Tipo, Descripcion from Bitacora where FechaYHora >= @FechaInicio and FechaYHora <= @FechaFin and Tipo = @Tipo", parametros);
                while (reader.Read())
                {
                    BE_Bitacora bitacora = new BE_Bitacora();
                    bitacora.Id = int.Parse(reader["Id"].ToString());
                    bitacora.Username = reader["Username"].ToString();
                    bitacora.FechaYHora = DateTime.Parse(reader["FechaYHora"].ToString());
                    bitacora.Tipo = reader["Tipo"].ToString();
                    bitacora.Descripcion = reader["Descripcion"].ToString();
                    lista.Add(bitacora);
                }
            }
            catch (Exception ex) { throw new Exception("DAL-BUSCAR BITACORA POR FECHAS Y TIPO - " + ex.Message); }
            finally { acceso.Cerrar(); }
            return lista;
        }
    }
}
