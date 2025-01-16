using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Sistemas
    {
        public Sistemas()
        {
            Proyectos = new HashSet<Proyectos>();
        }

        public int IdSistema { get; set; }
        public string NombreSistema { get; set; }
        public int IdRelacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstatusActivo { get; set; }

        public virtual RelacionClientesUnidadesAreas IdRelacionNavigation { get; set; }
        public virtual ICollection<Proyectos> Proyectos { get; set; }
    }
}
