using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class CatUsuarios
    {
        public CatUsuarios()
        {
            BitacoraH = new HashSet<BitacoraH>();
            RelacionUsuarioEmpleado = new HashSet<RelacionUsuarioEmpleado>();
            RelacionUsuarioEmail = new HashSet<RelacionUsuarioEmail>();
            HistorialB = new HashSet<HistorialB>();
            RelUsuarioAreaUnidad = new HashSet<RelacionUsuarioUnidadArea>();
        }

        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }        
        public int IdRol { get; set; }
        public int? IdUsrRegistro { get; set; }
        public int? IdUsrModificacion { get; set; }
        public int? IdUsrElimino { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Estatus{ get; set; }
        public int? Temporal { get; set; }
        public int? RegistroBitacora { get; set; }

        public virtual CatRoles IdRolNavigation { get; set; }
        public virtual ICollection<BitacoraH> BitacoraH { get; set; }
        public virtual ICollection<RelacionUsuarioEmpleado> RelacionUsuarioEmpleado { get; set; }
        public virtual ICollection<RelacionUsuarioEmail> RelacionUsuarioEmail { get; set; }
        public virtual ICollection<HistorialB> HistorialB { get; set; }
        public virtual ICollection<RelacionUsuarioUnidadArea> RelUsuarioAreaUnidad { get; set; }
        public virtual ICollection<TblRecordatorios> TblRecordatorios { get; set; }

    }
}
