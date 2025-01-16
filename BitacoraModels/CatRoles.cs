using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class CatRoles
    {
        public CatRoles()
        {
            RelRolPantalla = new HashSet<RelacionRolPantalla>();
            CatUsuarios = new HashSet<CatUsuarios>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Estatus { get; set; }

        public virtual ICollection<RelacionRolPantalla> RelRolPantalla { get; set; }
        public virtual ICollection<CatUsuarios> CatUsuarios { get; set; }
    }
}
