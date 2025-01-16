using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class RelacionUnidadArea
    {
        public RelacionUnidadArea()
        {
            RelacionClientesUnidadesAreas = new HashSet<RelacionClientesUnidadesAreas>();
        }

        public int Id { get; set; }
        public int IdUnidadNegocio { get; set; }
        public int IdAreaNegocio { get; set; }

        public virtual CatAreasNegocio IdAreaNegocioNavigation { get; set; }
        public virtual CatUnidadesNegocios IdUnidadNegocioNavigation { get; set; }
        public virtual ICollection<RelacionClientesUnidadesAreas> RelacionClientesUnidadesAreas { get; set; }
    }
}
