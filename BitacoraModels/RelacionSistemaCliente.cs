using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class RelacionSistemaCliente
    {

        public int Id { get; set; }
        public int IdSistema { get; set; }
        public int IdCliente{ get; set; }

        public virtual CatSistemas IdSistemaNavigation { get; set; }
        public virtual CatClientes IdClienteNavigation { get; set; }

    }
}
