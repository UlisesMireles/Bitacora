using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class CatActividades
    {
        public CatActividades()
        {
            BitacoraH = new HashSet<BitacoraH>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Estatus { get; set; }
        public string Evento { get; set; }

        public virtual ICollection<BitacoraH> BitacoraH { get; set; }
    }
}
