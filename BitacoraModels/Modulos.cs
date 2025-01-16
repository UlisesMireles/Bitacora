using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Modulos
    {
        public Modulos()
        {
            UsuariosModulos = new HashSet<UsuariosModulos>();
        }

        public int IdModulo { get; set; }
        public string NombreModulo { get; set; }
        public string Url { get; set; }
        public string UrlHijo { get; set; }

        public virtual ICollection<UsuariosModulos> UsuariosModulos { get; set; }
    }
}
