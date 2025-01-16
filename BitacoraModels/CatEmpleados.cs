using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class CatEmpleados
    {
        public CatEmpleados()
        {
            RelacionUsuarioEmpleado = new HashSet<RelacionUsuarioEmpleado>();
            RelacionProyectoEmpleado = new HashSet<RelacionProyectoEmpleado>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Iniciales { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string EmailInterno { get; set; }
        public string Estatus { get; set; }
        public int IdUnidad { get; set; }
        public string EmailAsignado { get; set; }
        public int EstatusERT { get; set; }
        public string Foto { get; set; }

        public virtual CatUnidadesNegocios IdUnidadNavigation { get; set; }
        public virtual TblCatEstatusEmpleado IdEstatusNavigation { get; set; }
        public virtual ICollection<RelacionUsuarioEmpleado> RelacionUsuarioEmpleado { get; set; }
        public virtual ICollection<RelacionProyectoEmpleado> RelacionProyectoEmpleado { get; set; }
    }
}
