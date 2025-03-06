using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RelacionProyectos
    {
        public int IdProyecto { get; set; }
        public int IdCliente { get; set; }
        public int IdSistema { get; set; }
        public int IdUnidad { get; set; }
        public int IdArea { get; set; }
        public string Estatus { get; set; }
        public int IdEstatusProceso { get; set; }

        public virtual CatProyectos IdProyectoNavigation { get; set; }
        public virtual CatClientes IdClienteNavigation { get; set; }
    }
}
