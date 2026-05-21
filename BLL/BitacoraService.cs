using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class BitacoraService
    {
        private readonly static DAL_Bitacora b = new DAL_Bitacora();
        public static int Crear(BE_Bitacora bitacora)
        {
            try
            {
                int resultado = b.Crear(bitacora);
                if (resultado == 0) throw new Exception("No se creó la bitácora");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("CREAR BITACORA - " + ex.Message);
            }
        }
        public static List<BE_Bitacora> Buscar(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return b.Buscar(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                throw new Exception("BUSCAR BITACORA POR FECHAS - " + ex.Message);
            }
        }
        public static List<BE_Bitacora> Buscar(string tipo, DateTime fechaInicio, DateTime fechafin)
        {
            try
            {
                return b.Buscar(tipo, fechaInicio, fechafin);
            }
            catch (Exception ex)
            {
                throw new Exception("BUSCAR BITACORA POR FECHAS Y TIPO - " + ex.Message);
            }
        }
    }
}
