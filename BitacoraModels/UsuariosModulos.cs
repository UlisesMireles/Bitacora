using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class UsuariosModulos
    {
        public int IdUsr { get; set; }
        public int IdModulo { get; set; }

        public virtual Modulos IdModuloNavigation { get; set; }
    }
}
