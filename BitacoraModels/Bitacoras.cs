using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Bitacoras
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsr { get; set; }
        public int? IdProyecto { get; set; }
        public int? IdEtapa { get; set; }
        public int IdActividad { get; set; }
        public string Descripcion { get; set; }
        public decimal Duracion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual CatActividades IdActividadNavigation { get; set; }
        public virtual CatUsuarios IdUsrNavigation { get; set; }
    }
}
