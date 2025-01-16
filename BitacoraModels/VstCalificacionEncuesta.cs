using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class VstCalificacionEncuesta
    {
        public int IdEncuesta{ get; set; }
        public int Idempleado { get; set; }
        public string Empleado { get; set; }
        public int Puntos { get; set; }
        public string DesCal { get; set; }
    }
}
