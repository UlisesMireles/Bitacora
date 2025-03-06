using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitacoraLogic;
using BitacoraModels;
using log4net;

namespace Bitacora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(UsuariosController));
        UsuariosLogic _UsuariosLogic = new UsuariosLogic();

        [HttpGet("[action]/{id}")]
        public object ConsultaUsuarios()
        {
            _log.Info("Bitacora Controller Llega Controller");
            var Usuarios = _UsuariosLogic.ConsultaUsuarios();
            var resp = new { result = "", Usuarios = Usuarios };

            _log.Info("Bitacora Controller return");
            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaEmpleados()
        {
            var Empleados = _UsuariosLogic.ConsultaEmpleados();
            var resp = new { result = "", Empleados = Empleados};

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaRoles()
        {
            var Roles = _UsuariosLogic.ConsultaRoles();
            var resp = new { result = "", TipoUsRolesuario = Roles };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object GeneraUsuario(int idEmpleado)
        {
            var Usuario = _UsuariosLogic.GeneraUsuario(idEmpleado);
            var resp = new { usuario = Usuario };
            return resp;
        }

        [HttpPost("[action]/{id}")]
        public int InsertaUsuario(Usuarios datos)
        {           
            var idUsuer = _UsuariosLogic.InsertaUsuario(datos);
            return idUsuer;
        }

        [HttpPost("[action]")]
        public int ModificaUsuario(Usuarios datos)
        {
            var idUsuer = _UsuariosLogic.ModificaUsuario(datos);
            return idUsuer;
        }

        [HttpGet("[action]/{id}")]
        public int BajaUsuario(int idUser)
        {
            var idUsuer = _UsuariosLogic.BajaUsuario(idUser);
            return idUsuer;
        }

        [HttpGet("[action]/{id}")]
        public int RestablecePassword(int idUser)
        {
            var idUsuer = _UsuariosLogic.RestablecePassword(idUser);
            return idUsuer;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaRel_UserUnArea(int idUser)
        {
            var ListaRelacion = _UsuariosLogic.ConsultaRel_UserUnArea(idUser);
            var resp = new { ListaRelacion = ListaRelacion };

            return resp;
        }

        [HttpGet("[action]")]
        public object ConsultaRolesUsuarios()
        {
            _log.Info("Bitacora Controller ConsultaRolesUsuarios Llega Controller");
            var listaRolesUsuarios = _UsuariosLogic.ConsultaRolesUsuarios();
            var resp = new { result = "", listaRolesUsuarios = listaRolesUsuarios };

            _log.Info("Bitacora Controller return ConsultaRolesUsuarios");
            return resp;
        }

        [HttpGet("[action]")]
        public object ConsultaEstatusERTUsuarios()
        {
            _log.Info("Bitacora Controller ConsultaEstatusERTUsuarios Llega Controller");

            var listaEstatusERTUsuarios = _UsuariosLogic.ConsultaEstatusERTUsuarios();

            // Filtrar los elementos donde estatusERT no sea null ni vacío
            var listaFiltrada = listaEstatusERTUsuarios
                .Where(u => !string.IsNullOrEmpty(u.EstatusERT))
                .ToList();

            var resp = new { result = "", listaEstatusERTUsuarios = listaFiltrada };

            _log.Info("Bitacora Controller return ConsultaEstatusERTUsuarios");
            return resp;
        }

    }
}