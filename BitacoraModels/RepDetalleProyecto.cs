using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RepDetalleProyecto
    {
        public int Index { get; set; }
        public int IdUsr { get; set; }
        public string Usuario { get; set; }
        public int? IdProyecto { get; set; }
        public decimal TotalHoras { get; set; }
        public string Proyecto { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public List<BItacoraInf> Registros { get; set; }

    }
}
