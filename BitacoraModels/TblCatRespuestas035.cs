using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblCatRespuestas035
    {
        public TblCatRespuestas035()
        {
            TblValorPreguntasNom35 = new HashSet<TblValorPreguntasNom35>();
        }

        public int IdRespuesta { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TblValorPreguntasNom35> TblValorPreguntasNom35 { get; set; }
    }
}
