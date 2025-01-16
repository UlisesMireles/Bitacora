using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class TblCatOpcionesAccionDominio
    {
        public int IdResDominio { get; set; }
        public int IdDominio { get; set; }
        public int ValorMin { get; set; }
        public int ValorMax { get; set; }
        public int IdCal { get; set; }

        public virtual TblCatOpciones IdCalNavigation { get; set; }
        public virtual TblDominioNom35 IdDominioNavigation { get; set; }
    }
}
