using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BitacoraModels
{
    public class DatosReporte
    {
        public int IdUser { get; set; }
        public int IdUnidad { get; set; }
        public int IdCliente { get; set; }
        public int IdArea { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public int IdUserFiltro { get; set; }
        public int IdProyecto { get; set; }
        public int IdEtapa { get; set; }
        public int IdActividad { get; set; }
        public int IdUnidadUsuario { get; set; }
        public string varDetalle { get; set; }
    }
}
