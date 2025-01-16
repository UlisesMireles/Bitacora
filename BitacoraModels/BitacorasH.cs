using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class BitacorasH
    {
        public int Id { get; set; }
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsr { get; set; }
        public int IdProyecto { get; set; }
        public int IdFase { get; set; }
        public int IdActividad { get; set; }
        public string Descripcion { get; set; }
        public decimal Duracion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Movimiento { get; set; }
    }
}
