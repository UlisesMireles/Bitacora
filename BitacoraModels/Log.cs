using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Log
    {
        public string Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Sp { get; set; }
        public string Parametros { get; set; }
        public int Id { get; set; }
    }
}
