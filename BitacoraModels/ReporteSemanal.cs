using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class ReporteSemanal
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Usuario { get; set; }
        public int IdProyecto { get; set; }
        public string Proyecto { get; set; }
        public string Actividad { get; set; }
        public int IdUnidad { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string Unidad { get; set; }
        public int IdArea { get; set; }
        public string Area { get; set; }
        public decimal Horas { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
