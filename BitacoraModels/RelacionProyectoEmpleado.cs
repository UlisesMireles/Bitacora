using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RelacionProyectoEmpleado
    {
        public int Id{ get; set; }
        public int IdProyecto { get; set; }
        public int IdEmpleado { get; set; }
        public int IdRol { get; set; }

        public virtual CatProyectos IdProyectoNavigation { get; set; }
        public virtual CatEmpleados IdEmpleadoNavigation { get; set; }
    }
}
