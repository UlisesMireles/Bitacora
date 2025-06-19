using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitacoraModels
{
    public class RespuestaOpenIA
    {
        public string? Respuesta { get; set; }
        public int TokensEntrada { get; set; }
        public int TokensSalida { get; set; }
        public float[]? PreguntaVector { get; set; }
    }
}
