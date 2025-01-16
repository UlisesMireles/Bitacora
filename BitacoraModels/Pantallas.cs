using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class Pantallas
    {
        public Pantallas()
        {
            RelRolantalla = new HashSet<RelacionRolPantalla>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdMenu { get; set; }


        public virtual Menu IdMenuNavigation { get; set; }
        public virtual ICollection<RelacionRolPantalla> RelRolantalla { get; set; }
    }
}
