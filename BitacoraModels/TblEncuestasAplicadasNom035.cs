using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblEncuestasAplicadasNom035
    {
        public int IdEncApli { get; set; }
        public int? IdEncuesta { get; set; }
        public int? Idempleado { get; set; }
        public int? IdPregunta { get; set; }
        public int? Idrespuestausuario { get; set; }
        public int? Puntos { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual TblPreguntasNom35 IdPreguntaNavigation { get; set; }
    }
}
