using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Proyectos
    {
        public int IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public int IdSistema { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstatusActivo { get; set; }
        public short? Orden { get; set; }
        public int? IdUnidad { get; set; }
        public string DescripcionNegocio { get; set; }
        public string DescripcionTecnica { get; set; }
        public string Archivar { get; set; }
        public string Ot { get; set; }

        public virtual Sistemas IdSistemaNavigation { get; set; }
    }
}
