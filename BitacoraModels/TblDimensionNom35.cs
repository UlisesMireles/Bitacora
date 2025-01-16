using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblDimensionNom35
    {
        public TblDimensionNom35()
        {
            TblPreguntasNom35 = new HashSet<TblPreguntasNom35>();
        }

        public int IdDimension { get; set; }
        public string DesDimension { get; set; }

        public virtual ICollection<TblPreguntasNom35> TblPreguntasNom35 { get; set; }
    }
}
