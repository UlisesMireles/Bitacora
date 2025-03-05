using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitacoraLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitacoraModels;
using Bitacora.Helpers;

namespace Bitacora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaCatalogosController : ControllerBase
    {
        ConsultaCatalogosLogic _ConsultaCatalogo = new ConsultaCatalogosLogic();

        [HttpGet("[action]/{id}")]
        public object ConsultaUnidadesNegocio()
        {
            var UnidadesNegocio = _ConsultaCatalogo.ConsultaUnidadesNegocio();
            var resp = new { result = "", UnidadesNegocio = UnidadesNegocio };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaAreasNegocio()
        {           
            var AreasNegocio = _ConsultaCatalogo.ConsultaAreasNegocio();
            var resp = new { result = "", AreasNegocio = AreasNegocio };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaClientes()
        {
            var Clientes = _ConsultaCatalogo.ConsultaClientes();
            var resp = new { result = "", Clientes = Clientes };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaSistemas()
        {
            var Sistemas = _ConsultaCatalogo.ConsultaSistemas();
            var resp = new { result = "", Sistemas = Sistemas };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaEtapas()
        {
            var Etapas = _ConsultaCatalogo.ConsultaEtapas();
            var resp = new { result = "", Etapas = Etapas };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaActividades()
        {
            var Actividades = _ConsultaCatalogo.ConsultaActiviades();
            var resp = new { result = "", Actividades = Actividades };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaProyectos()
        {
            var Proyectos = _ConsultaCatalogo.ConsultaProyectos();
            var resp = new { result = "", Proyectos = Proyectos };

            return resp;
        }

        [HttpGet("[action]")]
        public object ConsultaEstatusProceso()
        {
            var listaEstatusProceso = _ConsultaCatalogo.ConsultaEstatusProceso();

            var listaFiltrada = listaEstatusProceso
                .Where(u => !string.IsNullOrEmpty(u.EstatusProceso))
                .ToList();

            var resp = new { result = "", listaEstatusProceso = listaFiltrada };

            return resp;           
           
        }
    }
}