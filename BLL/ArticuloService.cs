using System;
using System.Collections.Generic;

using BE;
using DAL;

namespace BLL
{
    public static class ArticuloService
    {
        private readonly static MapperArticulo mapper = new MapperArticulo();

        public static int Crear(Articulo articulo)
        {
            try
            {
                ValidarArticulo(articulo);

                if (articulo is Lote lote)
                {
                    ValidarLote(lote);
                }

                int resultado = mapper.Insertar(articulo);

                if (resultado == 0)
                {
                    throw new Exception("No se creó el artículo");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("CREAR ARTICULO - " + ex.Message);
            }
        }

        public static int Editar(Articulo articulo)
        {
            try
            {
                ValidarArticulo(articulo);

                if (articulo.Id <= 0)
                {
                    throw new Exception("El artículo no tiene un Id válido");
                }

                if (articulo is Lote lote)
                {
                    ValidarLote(lote);
                }

                int resultado = mapper.Editar(articulo);

                if (resultado == 0)
                {
                    throw new Exception("No se editó el artículo");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("EDITAR ARTICULO - " + ex.Message);
            }
        }

        public static int Borrar(Articulo articulo)
        {
            try
            {
                if (articulo == null)
                {
                    throw new Exception("El artículo no puede ser nulo");
                }

                if (articulo.Id <= 0)
                {
                    throw new Exception("El artículo no tiene un Id válido");
                }

                int resultado = mapper.Borrar(articulo);

                if (resultado == 0)
                {
                    throw new Exception("No se borró el artículo");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("BORRAR ARTICULO - " + ex.Message);
            }
        }

        public static List<Articulo> Listar()
        {
            try
            {
                return mapper.Listar();
            }
            catch (Exception ex)
            {
                throw new Exception("LISTAR ARTICULOS - " + ex.Message);
            }
        }

        private static void ValidarArticulo(Articulo articulo)
        {
            if (articulo == null)
            {
                throw new Exception("El artículo no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(articulo.Nombre))
            {
                throw new Exception("El nombre es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(articulo.Descripcion))
            {
                throw new Exception("La descripción es obligatoria");
            }

            if (!(articulo is Lote) && articulo.Precio <= 0)
            {
                throw new Exception("El precio debe ser mayor a cero");
            }
        }

        private static void ValidarLote(Lote lote)
        {
            if (lote.Articulos == null || lote.Articulos.Count == 0)
            {
                throw new Exception("El lote debe tener al menos un artículo");
            }

            foreach (Articulo articulo in lote.Articulos)
            {
                if (articulo == null)
                {
                    throw new Exception("El lote contiene un artículo inválido");
                }

                if (articulo.Id <= 0)
                {
                    throw new Exception("Todos los artículos del lote deben estar guardados previamente");
                }

                if (articulo.Id == lote.Id)
                {
                    throw new Exception("Un lote no puede contenerse a sí mismo");
                }
            }
        }
    }
}
