using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
   public class CatEtapas
    {        
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Estatus { get; set; }        
    }
}
