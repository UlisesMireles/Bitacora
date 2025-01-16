using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RelacionUsuarioEmail
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }

        public virtual CatUsuarios IdUserNavigation { get; set; }
    }
}
