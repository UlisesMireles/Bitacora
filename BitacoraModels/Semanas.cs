using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class Semanas
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TotalHoras { get; set; }
    }
}
