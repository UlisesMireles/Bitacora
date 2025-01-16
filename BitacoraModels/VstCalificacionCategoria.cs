using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public partial class VstCalificacionCategoria
    {
        public int IdEncuesta { get; set; }
        public int Idempleado { get; set; }
        public string Empleado { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public int Puntos { get; set; }
        public int IdCal{ get; set; }
        public string DesCal { get; set; }
        public int numPreguntas { get; set; }
        public string rangoPreguntas { get; set; }
        public int nullDespreciable { get; set; }
        public int bajo { get; set; }
        public int medio { get; set; }
        public int alto { get; set; }
        public int muyAlto { get; set; }
        public string nullDespreciableCadena { get; set; }
        public string bajoCadena { get; set; }
        public string medioCadena { get; set; }
        public string altoCadena { get; set; }
        public string muyAltoCadena { get; set; }
        public string calificacion { get; set; }
    }

    public partial class rangos
    {
        public int valorMin { get; set; }
        public int valorMax { get; set; }
     
    }
}
