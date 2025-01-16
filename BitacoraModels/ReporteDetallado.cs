using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class ReporteDetallado
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Horas { get; set; }
        public int IdUser { get; set; }
        public string Usuario { get; set; }
        public int IdProyecto { get; set; }
        public string Proyecto { get; set; }
        public int IdUnidad { get; set; }
        public string Unidad { get; set; }
        public int IdArea { get; set; }
        public string Area { get; set; }
        public int IdActividad { get; set; }
        public string Actividad { get; set; }
        public string Detalle { get; set; }
        public int IdEtapa { get; set; }
        public string Etapa { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int IdUnidadUsuario { get; set; }
        public string UnidadUsuario { get; set; }
    }
}
