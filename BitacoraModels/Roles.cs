using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class Roles
    {
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public string Descripcion { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public List<string> ListPantallas { get; set; }
        public List<int> IdPantallas { get; set; }
    }
}
