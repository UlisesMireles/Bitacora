using System;
using System.Collections.Generic;

namespace BitacoraModels
{
    public partial class Usuarios
    {
        public int IdUser { get; set; }
        public string Usuario { get; set; }
        public string Estatus { get; set; }
        public int IdRol{ get; set; }
        public int IdUnidad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdUsrRegistro { get; set; }
        public int? IdUsrModificacion { get; set; }
        public int? IdUsrElimino { get; set; }
        public string Password { get; set; }
        public String Nombre { get; set; }
        public string Rol { get; set; }
        public int IdEmpleado { get; set; }
        public string Email { get; set; }
        public int IdRelacion { get; set; }
        public string EstausEmpleado { get; set; }
        public int Temporal { get; set; }
        public int Registro { get; set; }
        public string EstatusERT { get; set; }
        public string EmailAsignado { get; set; }
        public string Foto { get; set; }
        
        public List<RelacionUsuarioUnidadArea> ListaUnidadArea { get; set; }
    }
}
