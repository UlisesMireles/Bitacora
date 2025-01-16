using System;
using System.Collections.Generic;
using System.Text;

namespace BitacoraModels
{
    public class Nom035Model
    {
        public UsuarioNom035 infoUsuario { get; set; }
        public List<respuestas> respuestas { get; set; }

        public List<UsuarioNom035> participacion { get; set; }
    }
    public class UsuarioNom035
    {
        public int id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPat { get; set; }
        public string AperllidoMat { get; set; }
        public int AplicarExamen { get; set; }
        public string email { get; set; }
        public DateTime? fecha { get; set; }
        public int encuestados { get; set; }
        public int noEncuestados { get; set; }
        public int Recordatorio { get; set; }
    }
    public class respuestas
    {
        public int idempleado { get; set; }
        public int IdPregunta { get; set; }
        public int Idrespuestausuario { get; set; }
        public int? puntos { get; set; }
        public DateTime fecha { get; set; }
    }

}
