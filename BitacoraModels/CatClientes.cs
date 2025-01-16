using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class CatClientes
    {
        public CatClientes()
        {
            RelacionSistemaCliente = new HashSet<RelacionSistemaCliente>();
            RelacionProyectos = new HashSet<RelacionProyectos>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Alias { get; set; }
        public string Rfc { get; set; }
        public string ClaveFolio { get; set; }
        public string PorcRentaEsperadaA { get; set; }
        public string PorcRentaEsperadaDe { get; set; }
        public int? HorasAsignacion { get; set; }
        public int? DiasLimitePago { get; set; }
        public string Giro { get; set; }
        public string Domicilio { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Estatus { get; set; }
        public string Unidad { get; set; }
        public string Area { get; set; }
        
        public virtual ICollection<RelacionSistemaCliente> RelacionSistemaCliente { get; set; }
        public virtual ICollection<RelacionProyectos> RelacionProyectos { get; set; }
    }
}
