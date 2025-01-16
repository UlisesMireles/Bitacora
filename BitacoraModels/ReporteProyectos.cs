using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class ReporteProyectos
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
        public int IdProyecto { get; set; }
        public string Proyecto { get; set; }
        public int IdActividad { get; set; }
        public string Actividad { get; set; }
        public decimal Horas { get; set; }
        public int IdUnidad { get; set; }
        public string Unidad { get; set; }
        public int IdArea { get; set; }
        public string Area { get; set; }
        public int IdUnidadRegistro { get; set; }
        public string UnidadRegistro { get; set; }

    }
}
