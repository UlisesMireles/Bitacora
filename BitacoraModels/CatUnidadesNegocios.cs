using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class CatUnidadesNegocios
    {
        public CatUnidadesNegocios()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
            RelUsuarioAreaUnidad = new HashSet<RelacionUsuarioUnidadArea>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Estatus { get; set; }
        public string Area { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
        public virtual ICollection<RelacionUsuarioUnidadArea> RelUsuarioAreaUnidad { get; set; }
    }
}
