using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class Menu
    {
        public Menu()
        {
            Pantalla = new HashSet<Pantallas>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int orden { get; set; }

        public virtual ICollection<Pantallas> Pantalla { get; set; }
    }
}
