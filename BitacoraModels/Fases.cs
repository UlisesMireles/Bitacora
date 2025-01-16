using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Fases
    {
        public int IdFase { get; set; }
        public string NombreFase { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstatusActivo { get; set; }
        public short? Orden { get; set; }
     }
}
