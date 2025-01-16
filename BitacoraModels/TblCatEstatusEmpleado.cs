using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class TblCatEstatusEmpleado
    {
        public TblCatEstatusEmpleado()
        {
            CatEmpleados = new HashSet<CatEmpleados>();
        }
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual ICollection<CatEmpleados> CatEmpleados { get; set; }
    }
}
