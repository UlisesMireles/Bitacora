using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitacoraLogic;
using BitacoraModels;
using Microsoft.AspNetCore.Mvc;

namespace Bitacora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Nom035Controller : Controller
    {
        Nom035Logic _Nom035Logic = new Nom035Logic();

        [HttpPost("[action]")]
        public object Resultados(Nom035Model info)
        {
            
            return _Nom035Logic.resultados(info); ;
        }

        [HttpPost("[action]")]
        public ActionResult Infousuario(UsuarioNom035 info)
        {
           
            var data = _Nom035Logic.InformacionUsuario(info.Usuario);
            return new OkObjectResult(data); ;
        }
        [HttpPost("[action]")]
        public object consultaEncuestados()
        {
            var listaUsuarioNom035 = _Nom035Logic.consultaEncuestados();

            var resp = new { result = "", Encuestados = listaUsuarioNom035 };

            return resp;
        }
        [HttpPost("[action]")]
        public object consultaNoEncuestados()
        {
            var listaUsuarioNom035 = _Nom035Logic.consultaNoEncuestados();
            var resp = new { result = "", Encuestados = listaUsuarioNom035 };
           return resp;
        }
        [HttpPost("[action]")]
        public object RecordatorioNom035(UsuarioNom035 usuarios)
        {
            int envio = _Nom035Logic.RecordatorioNom035(usuarios.id, usuarios.email);
            var resp = new { result = "", Recordatorio = envio };
            return resp;
        }
        [HttpPost("[action]")]
        public ActionResult InformacionEncuestas()
        {
            var data = _Nom035Logic.InformacionEncuestas();
            return new OkObjectResult(data); ;
        }

        [HttpPost("[action]")]
        public ActionResult ConsultaResultadosEncuestas()
        {
            var data = _Nom035Logic.ConsultaResultadosEncuestas();
            return new OkObjectResult(data); ;
        }
        [HttpPost("[action]")]
        public ActionResult ConsultaResultadosEncuestasCategoria()
        {
            var data = _Nom035Logic.ConsultaResultadosEncuestasCategoria();
            return new OkObjectResult(data); ;
        }

        [HttpPost("[action]")]
        public ActionResult ConsultaResultadosEncuestasDominio()
        {
            var data = _Nom035Logic.ConsultaResultadosEncuestasDominio();
            return new OkObjectResult(data); ;
        }
        [HttpPost("[action]")]
        public ActionResult ConResultEncuestasCategoriaPorEmpleado(VstCalificacionCategoria datos)
        {
            var data = _Nom035Logic.ConResultEncuestasCategoriaPorEmpleado(datos.Idempleado);
            return new OkObjectResult(data); ;
        }

        [HttpPost("[action]")]
        public ActionResult ConResultEncuestasDominioPorEmpleado(VstCalificacionDominio datos )
        {
            var data = _Nom035Logic.ConResultEncuestasDominioPorEmpleado(datos.Idempleado);
            return new OkObjectResult(data); ;
        }

        [HttpPost("[action]")]
        public ActionResult ConResultEncuestasCategoriaPorEmpresa()
        {
            var data = _Nom035Logic.ConResultEncuestasCategoriaPorEmpresa();
            return new OkObjectResult(data); ;
        }

        [HttpPost("[action]")]
        public ActionResult ConResultEncuestasDominioPorEmpresa()
        {
            var data = _Nom035Logic.ConResultEncuestasDominioPorEmpresa();
            return new OkObjectResult(data); ;
        }
        [HttpPost("[action]")]
        public ActionResult ConsultaResultadosEncuestasTotales()
        {
            var data = _Nom035Logic.ConsultaResultadosEncuestasTotalesLista();
            return new OkObjectResult(data); ;
        }
    }
}