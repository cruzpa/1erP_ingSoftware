using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public class MapperArticulo : Mapper<Articulo>
    {
        public override int Borrar(Articulo obj)
        {
            throw new NotImplementedException();
        }

        public override int Editar(Articulo obj)
        {
            throw new NotImplementedException();
        }

        public override int Insertar(Articulo obj)
        {
            throw new NotImplementedException();
        }

        //faltando probar jeje
        public override List<Articulo> Listar()
        {
            List<Articulo> articulos = new List<Articulo>();


            Acceso acceso = new Acceso();
            acceso.Abrir();
            string articulos_listar = "select distinct a.Id, a.Nombre, a.Descripcion, a.Precio," +
                " 'lote' = case when l.id is null then 'no' else 'si' end " +
                "from Articulo a " +
                "left join lote l on a.id = l.id";
            SqlDataReader reader = acceso.Leer(articulos_listar);
            while (reader.Read())
            {
                Articulo articulo = null;
                if (reader["lote"].ToString() == "si")
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
                
                articulos.Add(articulo);
            }
            string lotes_listar = "select * from lote";
            reader = acceso.Leer(lotes_listar);

            while (reader.Read())
            {
                Lote l = (from Articulo lote in articulos
                             where lote.Id == int.Parse(reader["id"].ToString())
                             select lote).First() as Lote;
                Articulo a = (from Articulo art in articulos
                             where art.Id == int.Parse(reader["id_articulo"].ToString())
                             select art).First();
                l.AgregarArticulo(a);

            }
            acceso.Cerrar();

            return articulos;
        }
    }
}
