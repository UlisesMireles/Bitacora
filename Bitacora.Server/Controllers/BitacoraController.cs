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
    public class BitacoraController : ControllerBase
    {
        Bitacora_Logic _BitacoraLogic = new Bitacora_Logic();

        [HttpGet("[action]/{id}")]
        public object ConsultaBitacora(int idUser)
        {
            var  listaBitacora = _BitacoraLogic.ConsultaBitacora(idUser);

            return listaBitacora;
        }

        [HttpGet("[action]/{id}")]
        public object GetProyectos(int idUser)
        {
            var Proyectos = _BitacoraLogic.GetProyectos(idUser);
            return Proyectos;
        }

        [HttpGet("[action]/{id}")]
        public object GetEtapas()
        {
            var Etapas = _BitacoraLogic.GetEtapas();
            return Etapas;
        }

        [HttpGet("[action]/{id}")]
        public object GetActividades()
        {
            var Actividades = _BitacoraLogic.GetActividades();
            return Actividades;
        }

        [HttpPost("[action]/{id}")]
        public int InsertaBitacora(BitacoraH datos)
        {
            var Bitacora = _BitacoraLogic.InsertaBitacora(datos);
            return Bitacora;
        }

        [HttpPost("[action]")]
        public int ModificarBitacora(BitacoraH datos)
        {
            var Bitacora = _BitacoraLogic.ModificaBitacora(datos);
            return Bitacora;
        }

        [HttpPost("[action]")]
        public int EliminarBitacora(BitacoraH datos)
        {
            var Bitacora = _BitacoraLogic.EliminarBitacora(datos.Id);
            return Bitacora;
        }

    }
}