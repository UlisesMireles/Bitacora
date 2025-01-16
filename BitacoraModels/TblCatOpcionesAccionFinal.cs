using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class TblCatOpcionesAccionFinal
    {
        public int IdResFinal { get; set; }
        public int ValorMin { get; set; }
        public int ValorMax { get; set; }
        public int IdCal { get; set; }

        public virtual TblCatOpciones IdCalNavigation { get; set; }
        
    }
}
