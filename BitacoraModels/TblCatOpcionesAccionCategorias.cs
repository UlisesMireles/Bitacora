using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class TblCatOpcionesAccionCategorias
    {
        public int IdResCategoria { get; set; }
        public int IdCategoria { get; set; }
        public int ValorMin { get; set; }
        public int ValorMax { get; set; }
        public int IdCal { get; set; }

        public virtual TblCatOpciones IdCalNavigation { get; set; }
        public virtual TblCategoriasNom35 IdCategoriaNavigation { get; set; }
    }
}
