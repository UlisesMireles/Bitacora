using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class CatProyectos
    {
        public CatProyectos()
        {
            RelacionProyectos = new HashSet<RelacionProyectos>();
            RelacionProyectoEmpleado = new HashSet<RelacionProyectoEmpleado>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public decimal? TotalHoras { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Cliente { get; set; }
        public string Sistema { get; set; }
        public string UnidadArea { get; set; }
        public string Estatus { get; set; }
        public int IdCategoria { get; set; }
        public string EstatusProceso { get; set; }

        public virtual CategoriasProyecto IdCategoriaNavigation { get; set; }
        public virtual ICollection<RelacionProyectos> RelacionProyectos { get; set; }
        public virtual ICollection<RelacionProyectoEmpleado> RelacionProyectoEmpleado { get; set; }
    }
}
