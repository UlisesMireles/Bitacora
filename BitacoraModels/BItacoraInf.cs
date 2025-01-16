using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class BItacoraInf
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsr { get; set; }
        public string Usuario { get; set; }
        public int? IdProyecto { get; set; }
        public int IdEtapa { get; set; }
        public int IdActividad { get; set; }
        public string Descripcion { get; set; }
        public decimal Duracion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Proyecto { get; set; }
        public string Activadad { get; set; }
        public string Etapa { get; set; }
        public int? IdUnidad { get; set; }
        public int? IdArea { get; set; }
    }
}
