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
    public class AvanceRealController : ControllerBase
    {
        AvanceRealLogic _AvanceRealLogic = new AvanceRealLogic();

        [HttpGet("[action]/{id}")]
        public object ConsultaUnidad(int idUser)
        {
            var Unidades = _AvanceRealLogic.ConsultaUnidades(idUser);
            var resp = new { result = "", AvanceR = Unidades };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaArea(int idUser)
        {
            var Areas = _AvanceRealLogic.ConsultaAreas(idUser);
            var resp = new { result = "", AvanceR = Areas };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaAreaRelacionada(int idUser, int idUnidad)
        {
            var Areas = _AvanceRealLogic.ConsultaAreaRelacionada(idUser, idUnidad);
            var resp = new { result = "", AvanceR = Areas };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaUnidadesRelacionadas(int idUser, int idArea)
        {
            var Areas = _AvanceRealLogic.ConsultaUnidadesRelacionadas(idUser, idArea);
            var resp = new { result = "", AvanceR = Areas };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object AvanceAvanceReal(int idUser, int idUnidad, int idArea)
        {
            var AvanceR = _AvanceRealLogic.AvanceReal(idUser, idUnidad, idArea);
            var resp = new { result = "", AvanceR = AvanceR };

            return resp;
        }

        [HttpPost("[action]")]
        public object ActualizarAvanceReal(List<AvanceRealList> listaAvance)
        {
              
            var AvanceR = _AvanceRealLogic.ActualizarAvanceReal(listaAvance);
            var resp = new { result = "", AvanceR = AvanceR };

            return resp;
        }


    }
       
}