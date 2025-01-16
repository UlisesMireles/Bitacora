using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class ReportePersonas
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Usuario { get; set; }
        public int IdUnidad { get; set; }
        public string Unidad { get; set; }
        public decimal Horas { get; set; }
        public int Estatus { get; set; }
    }
}
