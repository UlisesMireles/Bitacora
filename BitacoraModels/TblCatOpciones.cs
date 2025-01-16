using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class TblCatOpciones
    {
        public TblCatOpciones()
        {
            TblCatOpcionesAccionCategorias = new HashSet<TblCatOpcionesAccionCategorias>();
            TblCatOpcionesAccionDominio = new HashSet<TblCatOpcionesAccionDominio>();
            TblCatOpcionesAccionFinal = new HashSet<TblCatOpcionesAccionFinal>();
        }
        public int IdCal { get; set; }
        public string DesCal { get; set; }
        public int Valor { get; set; }


        public virtual ICollection<TblCatOpcionesAccionCategorias> TblCatOpcionesAccionCategorias { get; set; }
        public virtual ICollection<TblCatOpcionesAccionDominio> TblCatOpcionesAccionDominio { get; set; }
        public virtual ICollection<TblCatOpcionesAccionFinal> TblCatOpcionesAccionFinal { get; set; }

    }
}
