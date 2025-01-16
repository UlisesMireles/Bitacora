using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class TblRecordatorios
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }

        public virtual CatUsuarios IdUsuarioNavigation { get; set; }
    }
}
