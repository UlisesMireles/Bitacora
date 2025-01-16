using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblCategoriasNom35
    {
        public TblCategoriasNom35()
        {
            TblPreguntasNom35 = new HashSet<TblPreguntasNom35>();
        }

        public int IdCategoria { get; set; }
        public string DescCategoria { get; set; }

        public virtual ICollection<TblPreguntasNom35> TblPreguntasNom35 { get; set; }
        public virtual ICollection<TblCatOpcionesAccionCategorias> TblCatOpcionesAccionCategorias { get; set; }
    }
}
