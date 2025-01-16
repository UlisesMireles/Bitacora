using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class AvanceRealList
    {
        public int IdProyecto { get; set; }
        public string NompreProyecto { get; set; }
        public int AvanceReal { get; set; }
        public decimal Horas { get; set; }           
        public int Avance { get; set; }
        public int IdUnidad { get; set; }
        public string Unidad { get; set; }
        public int? IdArea { get; set; }
        public string Area { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int Bandera { get; set; }
    }
}
