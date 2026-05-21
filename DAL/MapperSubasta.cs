using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class MapperSubasta : Mapper<Subasta>
    {
        private readonly Acceso acceso = new Acceso();
        private readonly MapperArticulo mapperArticulo = new MapperArticulo();
        private readonly DAL_Usuario dalUsuario = new DAL_Usuario();

        public override int Insertar(Subasta obj)
        {
            int resultado = 0;

            if (obj == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = CrearParametrosSubasta(obj);

                int idSubasta = acceso.LeerEscalar(
                    @"insert into Subasta
                    (
                        IdArticulo,
                        FechaInicio,
                        FechaFin,
                        PrecioInicial,
                        PrecioFinal,
                        IdGanador,
                        Estado
                    )
                    values
                    (
                        @IdArticulo,
                        @FechaInicio,
                        @FechaFin,
                        @PrecioInicial,
                        @PrecioFinal,
                        @IdGanador,
                        @Estado
                    );

                    select cast(scope_identity() as int);",
                    parametros
                );

                obj.Id = idSubasta;
                resultado = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-INSERTAR SUBASTA - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public override int Editar(Subasta obj)
        {
            int resultado = 0;

            if (obj == null) return resultado;

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = CrearParametrosSubasta(obj);
                parametros.Add(new SqlParameter("@Id", obj.Id));

                resultado = acceso.Escribir(
                    @"update Subasta
                    set
                        IdArticulo = @IdArticulo,
                        FechaInicio = @FechaInicio,
                        FechaFin = @FechaFin,
                        PrecioInicial = @PrecioInicial,
                        PrecioFinal = @PrecioFinal,
                        IdGanador = @IdGanador,
                        Estado = @Estado
                    where Id = @Id",
                    parametros
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-EDITAR SUBASTA - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public override int Borrar(Subasta obj)
        {
            int resultado = 0;

            if (obj == null) return resultado;

            acceso.Abrir();

            try
            {
                resultado = acceso.Escribir(
                    "delete from Subasta where Id = @Id",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@Id", obj.Id)
                    }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-BORRAR SUBASTA - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public override List<Subasta> Listar()
        {
            return ListarPorEstado(null);
        }

        public List<Subasta> ListarActivas()
        {
            return ListarPorEstado(EstadoSubasta.Activa);
        }

        public List<Subasta> ListarVigentes()
        {
            List<Articulo> articulos = mapperArticulo.Listar();
            List<Subasta> subastas = new List<Subasta>();

            acceso.Abrir();

            try
            {
                SqlDataReader reader = acceso.Leer(
                    @"select
                        Subasta.Id,
                        Subasta.IdArticulo,
                        Subasta.FechaInicio,
                        Subasta.FechaFin,
                        Subasta.PrecioInicial,
                        Subasta.PrecioFinal,
                        Subasta.IdGanador,
                        Subasta.Estado
                    from Subasta
                    inner join Articulo on Articulo.Id = Subasta.IdArticulo
                    where Subasta.FechaFin > getdate()
                    and Subasta.Estado = 'Activa'
                    and Articulo.Estado = 'EnSubasta'"
                );

                while (reader.Read())
                {
                    Subasta subasta = MapearSubasta(reader, articulos);

                    if (subasta != null)
                    {
                        subastas.Add(subasta);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-LISTAR SUBASTAS VIGENTES - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return subastas;
        }

        public void GuardarJornada(List<Subasta> subastas)
        {
            if (subastas == null)
            {
                throw new ArgumentNullException("subastas");
            }

            foreach (Subasta subasta in subastas)
            {
                if (subasta.Id <= 0)
                {
                    Insertar(subasta);
                }
                else
                {
                    Editar(subasta);
                }
            }
        }

        private List<Subasta> ListarPorEstado(EstadoSubasta? estado)
        {
            List<Articulo> articulos = mapperArticulo.Listar();
            List<Subasta> subastas = new List<Subasta>();

            acceso.Abrir();

            try
            {
                List<SqlParameter> parametros = null;
                string where = string.Empty;

                if (estado.HasValue)
                {
                    where = " where Estado = @Estado";
                    parametros = new List<SqlParameter>
                    {
                        acceso.CrearParametro("@Estado", estado.Value.ToString())
                    };
                }

                SqlDataReader reader = acceso.Leer(
                    @"select
                        Id,
                        IdArticulo,
                        FechaInicio,
                        FechaFin,
                        PrecioInicial,
                        PrecioFinal,
                        IdGanador,
                        Estado
                    from Subasta" + where,
                    parametros
                );

                while (reader.Read())
                {
                    Subasta subasta = MapearSubasta(reader, articulos);

                    if (subasta != null)
                    {
                        subastas.Add(subasta);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-LISTAR SUBASTAS - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return subastas;
        }

        private Subasta MapearSubasta(SqlDataReader reader, List<Articulo> articulos)
        {
            int idArticulo = Convert.ToInt32(reader["IdArticulo"]);
            Articulo articulo = articulos.FirstOrDefault(a => a.Id == idArticulo);

            if (articulo == null)
            {
                return null;
            }

            Subasta subasta = new Subasta(
                articulo,
                Convert.ToDateTime(reader["FechaInicio"]),
                Convert.ToDateTime(reader["FechaFin"])
            );

            subasta.Id = Convert.ToInt32(reader["Id"]);
            subasta.PrecioInicial = Convert.ToDecimal(reader["PrecioInicial"]);
            subasta.PrecioFinal = Convert.ToDecimal(reader["PrecioFinal"]);
            subasta.Estado = (EstadoSubasta)Enum.Parse(typeof(EstadoSubasta), reader["Estado"].ToString());

            if (reader["IdGanador"] != DBNull.Value)
            {
                subasta.MejorPostor = dalUsuario.BuscarPorId(Convert.ToInt32(reader["IdGanador"])) as Cliente;
            }

            return subasta;
        }

        private List<SqlParameter> CrearParametrosSubasta(Subasta subasta)
        {
            return new List<SqlParameter>
            {
                new SqlParameter("@IdArticulo", subasta.Articulo.Id),
                new SqlParameter("@FechaInicio", subasta.FechaInicio),
                new SqlParameter("@FechaFin", subasta.FechaFin),
                new SqlParameter("@PrecioInicial", subasta.PrecioInicial),
                new SqlParameter("@PrecioFinal", subasta.PrecioFinal),
                new SqlParameter("@IdGanador", subasta.MejorPostor != null ? (object)subasta.MejorPostor.Id : DBNull.Value),
                acceso.CrearParametro("@Estado", subasta.Estado.ToString())
            };
        }
    }
}
