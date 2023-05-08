using Microsoft.AspNetCore.Mvc;
using operacion_fuego_quasar.Models;
using operacion_fuego_quasar.Services;

namespace operacion_fuego_quasar.Controllers
{
    /// <summary>
    /// Controlador individual para los satellites.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class topsecret_splitController : ControllerBase
    {
        /// <summary>
        /// Metodo Get para la solicitud de informacion de los satellites.
        /// </summary>
        /// <returns>Respuesta con coordenadas y mensaje.</returns>
        /// <response code="200">Ok, Respuesta con coordenadas y mensaje.</response>
        /// <response code="404">No se pudo determinar la posicion o el mensaje.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(responseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public IActionResult Get()
        {
            if ((satelliteServices.coordenadasActuales.y == null && 
                satelliteServices.coordenadasActuales.y == null) || 
                string.IsNullOrEmpty(satelliteServices.ultimoMensaje))
            {
                return NotFound("No se pudo determinar la posicion o el mensaje.");
            }

            return Ok(new responseDTO()
            {
                message = satelliteServices.ultimoMensaje,
                position = satelliteServices.coordenadasActuales
            });
        }

        /// <summary>
        /// Metodo Post para el ingreso de informacion de cada satellite.
        /// </summary>
        /// <param name="satellite_name">Nombre del satellite.</param>
        /// <param name="data">Distancia y mensajes del satellite.</param>
        /// <returns>Respuesta con coordenadas y mensaje.</returns>
        /// <response code="200">Ok, Respuesta con coordenadas y mensaje.</response>
        /// <response code="400">Informacion de entrada invalida.</response>
        /// <response code="404">No se pudo determinar la posicion o el mensaje.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(responseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        [Route("{satellite_name}")]
        public IActionResult Post(string satellite_name, [FromBody] satelliteDTO data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Modelo invalido");
                }

                satellite_name = char.ToUpper(satellite_name[0]) + satellite_name.Substring(1).ToLower();
                if (!satelliteServices.mensajesRecibidos.ContainsKey(satellite_name))
                {
                    return BadRequest("Satelite desconocido");
                }

                satelliteServices.distanciasConocidas[satellite_name] = data.distance ?? 0;
                var distancias = new List<double>();
                foreach (var item in satelliteServices.distanciasConocidas)
                {
                    if (item.Value.HasValue)
                        distancias.Add(item.Value!.Value);
                }
                var coordenadas = satelliteServices.GetLocation(distancias.ToArray());

                satelliteServices.mensajesRecibidos[satellite_name] = data.message.ToList();
                var listMessages = new List<List<string>>();
                foreach (var item in satelliteServices.mensajesRecibidos)
                {
                    listMessages.Add(item.Value);
                }
                var mensaje = satelliteServices.GetMessage(listMessages);
                if ((coordenadas.y == null && coordenadas.y == null) || string.IsNullOrEmpty(mensaje))
                {
                    return NotFound("Informacion insuficiente, no se pudo determinar la posicion o el mensaje.");
                }
                return Ok(new responseDTO() { message = mensaje, position = coordenadas });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
