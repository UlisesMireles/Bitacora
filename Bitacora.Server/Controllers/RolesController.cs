using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitacoraLogic;
using BitacoraModels;

namespace Bitacora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        RolesLogic _RolesLogic = new RolesLogic();

        [HttpGet("[action]/{id}")]
        public object ConsultaRoles()
        {
            var Roles = _RolesLogic.ConsultaRoles();
            var resp = new { result = "", Roles = Roles };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaPantallas()
        {
            var Pantallas = _RolesLogic.ConsultaPantallas();
            var resp = new { result = "", Pantallas = Pantallas };

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object InsertaRol(Roles rol)
        {
            var Rol = _RolesLogic.InsertaRol(rol);
            var resp = new { result = "", Rol = Rol };

            return Rol;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaPantallasRol(int idRol)
        {
            var Pantallas = _RolesLogic.ConsultaPantallasRol(idRol);
            var resp = new { result = "", Pantallas = Pantallas };

            return resp;
        }

        [HttpPut("[action]/{id}")]
        public object ModificaRol(Roles rol)
        {
            var Rol = _RolesLogic.ModificaRol(rol);
            var resp = new { result = "", Rol = Rol };

            return resp;
        }

        [HttpDelete("[action]/{idRol}")]
        public object EliminaRol(int idRol)
        {
            var Rol = _RolesLogic.EliminaRol(idRol);
            var resp = new { result = "", Rol = Rol };

            return resp;
        }


    }
}