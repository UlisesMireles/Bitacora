using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblValorPreguntasNom35
    {
        public int IdValPreg { get; set; }
        public int? IdPregunta { get; set; }
        public int? IdRespuesta { get; set; }
        public int? Puntos { get; set; }

        public virtual TblCatRespuestas035 IdRespuestaNavigation { get; set; }
    }
}
