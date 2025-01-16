using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class CategoriasProyecto
    {
        public CategoriasProyecto()
        {
            CatProyectos = new HashSet<CatProyectos>();
        }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual ICollection<CatProyectos> CatProyectos { get; set; }
    }
}
