using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class ListReporteDetalladoProyectoPersona
    {
        public int Id { get; set; }
        public string Proyecto { get; set; }
        public string FechaInicioString { get; set; }
        public string FechaFinString { get; set; }
        public DateTime FechaInicioMoment { get; set; }
        public DateTime FechaFinMoment { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public decimal TotalHoras { get; set; }
    }
}
