using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblDominioNom35
    {
        public TblDominioNom35()
        {
            TblPreguntasNom35 = new HashSet<TblPreguntasNom35>();
        }

        public int IdDominio { get; set; }
        public string DesDominio { get; set; }

        public virtual ICollection<TblPreguntasNom35> TblPreguntasNom35 { get; set; }
        public virtual ICollection<TblCatOpcionesAccionDominio> TblCatOpcionesAccionDominio { get; set; }
    }
}
