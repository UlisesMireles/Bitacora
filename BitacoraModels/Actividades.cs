using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Actividades
    {
        public int IdActividad { get; set; }
        public string NombreActividad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstatusActivo { get; set; }

    }
}
