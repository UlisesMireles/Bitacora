using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class CatSistemas
    {
        public CatSistemas()
        {
            RelacionSistemaCliente = new HashSet<RelacionSistemaCliente>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Estatus { get; set; }
        public string Cliente { get; set; }

        public virtual ICollection<RelacionSistemaCliente> RelacionSistemaCliente { get; set; }
    }
}
