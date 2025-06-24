using BitacoraLogic.Utils.Asistentes;
using BitacoraModels;
using Microsoft.AspNetCore.Mvc;

namespace Bitacora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistentesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AsistentesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("OpenIA")]
        public async Task<ActionResult<ConsultaAsistente>> PostOpenIa(ConsultaAsistente consultaAsistente)
        {
            if (consultaAsistente == null || string.IsNullOrWhiteSpace(consultaAsistente.Pregunta))
            {
                return BadRequest("La consulta no puede estar vacía o la pregunta no puede ser nula.");
            }

            try
            {
                var asistente = new AsistenteHistorico(_configuration);
                var resultado = await asistente.AsistenteOpenAIAsync(consultaAsistente);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al procesar la solicitud: " + ex.Message);
            }
        }
    }
}
