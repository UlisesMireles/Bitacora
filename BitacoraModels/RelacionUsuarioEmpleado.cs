using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class RelacionUsuarioEmpleado
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdEmpleado { get; set; }

        public virtual CatEmpleados IdEmpleadoNavigation { get; set; }
        public virtual CatUsuarios IdUserNavigation { get; set; }
    }
}
