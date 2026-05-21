using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class MapperArticulo : Mapper<Articulo>
    {
        private readonly Acceso acceso = new Acceso();

        private Articulo MapearArticulo(SqlDataReader reader)
        {
            Articulo articulo;

            string tipo = reader["Tipo"].ToString();

            if (tipo == "Lote")
            {
                articulo = new Lote();
            }
            else
            {
                articulo = new Articulo();
            }

            articulo.Id = Convert.ToInt32(reader["Id"]);
            articulo.Nombre = reader["Nombre"].ToString();
            articulo.Descripcion = reader["Descripcion"].ToString();
            articulo.Precio = Convert.ToDecimal(reader["Precio"]);

            return articulo;
        }

        public override int Insertar(Articulo obj)
        {
            int resultado = 0;

            if (obj == null) return resultado;

            acceso.Abrir();
            acceso.IniciarTx();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Nombre", obj.Nombre),
                    acceso.CrearParametro("@Descripcion", obj.Descripcion),
                    acceso.CrearParametro("@Precio", Convert.ToSingle(obj.Precio)),
                    acceso.CrearParametro("@Tipo", obj.Tipo)
                };

                int idArticulo = acceso.LeerEscalar(
                    @"insert into Articulo
                    (
                        Nombre,
                        Descripcion,
                        Precio,
                        Tipo
                    )
                    values
                    (
                        @Nombre,
                        @Descripcion,
                        @Precio,
                        @Tipo
                    );

                    select cast(scope_identity() as int);",
                    parametros
                );

                obj.Id = idArticulo;

                if (obj is Lote lote)
                {
                    foreach (Articulo articulo in lote.Articulos)
                    {
                        List<SqlParameter> parametrosLote = new List<SqlParameter>
                        {
                            acceso.CrearParametro("@Id", lote.Id),
                            acceso.CrearParametro("@Id_Articulo", articulo.Id)
                        };

                        resultado = acceso.Escribir(
                            @"insert into Lote
                            (
                                Id,
                                Id_Articulo
                            )
                            values
                            (
                                @Id,
                                @Id_Articulo
                            )",
                            parametrosLote
                        );

                        if (resultado < 0)
                        {
                            throw new Exception("No se pudo insertar el detalle del lote");
                        }
                    }
                }

                acceso.ConfirmarTx();

                resultado = 1;
            }
            catch (Exception ex)
            {
                acceso.DeshacerTx();
                throw new Exception("DAL-INSERTAR ARTICULO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public override int Editar(Articulo obj)
        {
            int resultado = 0;

            if (obj == null) return resultado;

            acceso.Abrir();
            acceso.IniciarTx();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Id", obj.Id),
                    acceso.CrearParametro("@Nombre", obj.Nombre),
                    acceso.CrearParametro("@Descripcion", obj.Descripcion),
                    acceso.CrearParametro("@Precio", Convert.ToSingle(obj.Precio)),
                    acceso.CrearParametro("@Tipo", obj.Tipo)
                };

                resultado = acceso.Escribir(
                    @"update Articulo
                    set
                        Nombre = @Nombre,
                        Descripcion = @Descripcion,
                        Precio = @Precio,
                        Tipo = @Tipo
                    where Id = @Id",
                    parametros
                );

                if (resultado < 0)
                {
                    throw new Exception("No se pudo editar el artículo");
                }

                acceso.Escribir(
                    "delete from Lote where Id = @Id or Id_Articulo = @Id",
                    new List<SqlParameter>
                    {
                        acceso.CrearParametro("@Id", obj.Id)
                    }
                );

                if (obj is Lote lote)
                {
                    foreach (Articulo articulo in lote.Articulos)
                    {
                        List<SqlParameter> parametrosLote = new List<SqlParameter>
                        {
                            acceso.CrearParametro("@Id", lote.Id),
                            acceso.CrearParametro("@Id_Articulo", articulo.Id)
                        };

                        int resultadoLote = acceso.Escribir(
                            @"insert into Lote
                            (
                                Id,
                                Id_Articulo
                            )
                            values
                            (
                                @Id,
                                @Id_Articulo
                            )",
                            parametrosLote
                        );

                        if (resultadoLote < 0)
                        {
                            throw new Exception("No se pudo actualizar el detalle del lote");
                        }
                    }
                }

                acceso.ConfirmarTx();
            }
            catch (Exception ex)
            {
                acceso.DeshacerTx();
                throw new Exception("DAL-EDITAR ARTICULO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public override int Borrar(Articulo obj)
        {
            int resultado = 0;

            if (obj == null) return resultado;

            acceso.Abrir();
            acceso.IniciarTx();

            try
            {
                acceso.Escribir(
                    "delete from Lote where Id = @Id or Id_Articulo = @Id",
                    new List<SqlParameter>
                    {
                        acceso.CrearParametro("@Id", obj.Id)
                    }
                );

                resultado = acceso.Escribir(
                    "delete from Articulo where Id = @Id",
                    new List<SqlParameter>
                    {
                        acceso.CrearParametro("@Id", obj.Id)
                    }
                );

                if (resultado < 0)
                {
                    throw new Exception("No se pudo borrar el artículo");
                }

                acceso.ConfirmarTx();
            }
            catch (Exception ex)
            {
                acceso.DeshacerTx();
                throw new Exception("DAL-BORRAR ARTICULO - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return resultado;
        }

        public override List<Articulo> Listar()
        {
            List<Articulo> articulos = new List<Articulo>();

            acceso.Abrir();

            try
            {
                SqlDataReader reader = acceso.Leer(
                    @"select
                        Id,
                        Nombre,
                        Descripcion,
                        Precio,
                        Tipo
                    from Articulo"
                );

                while (reader.Read())
                {
                    articulos.Add(MapearArticulo(reader));
                }

                reader.Close();

                reader = acceso.Leer(
                    @"select
                        Id,
                        Id_Articulo
                    from Lote"
                );

                while (reader.Read())
                {
                    int idLote = Convert.ToInt32(reader["Id"]);
                    int idArticulo = Convert.ToInt32(reader["Id_Articulo"]);

                    Lote lote = articulos.FirstOrDefault(a => a.Id == idLote) as Lote;
                    Articulo articulo = articulos.FirstOrDefault(a => a.Id == idArticulo);

                    if (lote != null && articulo != null)
                    {
                        if (lote.Articulos == null)
                        {
                            lote.Articulos = new List<Articulo>();
                        }

                        lote.AgregarArticulo(articulo);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DAL-LISTAR ARTICULOS - " + ex.Message);
            }
            finally
            {
                acceso.Cerrar();
            }

            return articulos;
        }
    }
}