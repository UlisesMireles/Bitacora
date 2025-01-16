using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TiposUsuarios
    {
        public TiposUsuarios()
        {
            Usuarios = new HashSet<CatUsuarios>();
        }

        public int IdTipo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<CatUsuarios> Usuarios { get; set; }
    }
}
