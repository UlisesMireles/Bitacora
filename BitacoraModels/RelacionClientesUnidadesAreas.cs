using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class RelacionClientesUnidadesAreas
    {
        public RelacionClientesUnidadesAreas()
        {
            Sistemas = new HashSet<Sistemas>();
        }

        public int? IdRelacion { get; set; }
        public int IdCliente { get; set; }
        public int IdUnidad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstatusActivo { get; set; }

        public virtual CatClientes IdClienteNavigation { get; set; }
        public virtual RelacionUnidadArea IdUnidadNavigation { get; set; }
        public virtual ICollection<Sistemas> Sistemas { get; set; }
    }
}
