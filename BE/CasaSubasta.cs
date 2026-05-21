using System;
using System.Collections.Generic;

namespace BE
{
    public class CasaSubasta
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<Subasta> Subastas { get; set; }

        public CasaSubasta()
        {
            Subastas = new List<Subasta>();
        }
    }
}
