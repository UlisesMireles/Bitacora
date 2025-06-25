using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitacoraModels
{
    public class ConfiguracionDto
    {
        public int IdBot { get; set; }
        public string Asistente { get; set; } = string.Empty;
        public string NombreTablaAsistente { get; set; } = string.Empty;
        public string MensajePrincipalAsistente { get; set; } = string.Empty;
        public string Llave { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double CostoTokensEntrada { get; set; }
        public double CostoTokensSalida { get; set; }
        public int MaximoTokens { get; set; }
        public decimal Creditos { get; set; }
        public string RutaDocumento { get; set; } = string.Empty;
        public string? Prompt { get; set; } = string.Empty;
    }
}
