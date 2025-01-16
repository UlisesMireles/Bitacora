using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class HistorialB
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Pantalla { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public virtual CatUsuarios IdUsrNavigation { get; set; }
    }
}
