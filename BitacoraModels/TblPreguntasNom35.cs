using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class TblPreguntasNom35
    {
        public TblPreguntasNom35()
        {
            TblEncuestasAplicadasNom035 = new HashSet<TblEncuestasAplicadasNom035>();
        }

        public int IdPregunta { get; set; }
        public string DesPregunta { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdDominio { get; set; }
        public int? IdDimension { get; set; }

        public virtual TblCategoriasNom35 IdCategoriaNavigation { get; set; }
        public virtual TblDimensionNom35 IdDimensionNavigation { get; set; }
        public virtual TblDominioNom35 IdDominioNavigation { get; set; }
        public virtual ICollection<TblEncuestasAplicadasNom035> TblEncuestasAplicadasNom035 { get; set; }
    }
}
