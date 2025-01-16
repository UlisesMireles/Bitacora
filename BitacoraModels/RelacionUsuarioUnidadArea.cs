using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RelacionUsuarioUnidadArea
    {
        public int IdUser { get; set; }
        public int IdUnidad { get; set; }
        public int IdArea { get; set; }

        public virtual CatUsuarios IdUserNavigation { get; set; }
        public virtual CatUnidadesNegocios IdUnidadNavigation { get; set; }
        public virtual CatAreasNegocio IdAreaNavigation { get; set; }

    }
}
