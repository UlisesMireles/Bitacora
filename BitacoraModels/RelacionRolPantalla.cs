using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RelacionRolPantalla
    {
        public int IdRol { get; set; }
        public int IdPantalla{ get; set; }

        public virtual CatRoles IdRolNavigation { get; set; }
        public virtual Pantallas IdPantallaNavigation { get; set; }
    }
}
