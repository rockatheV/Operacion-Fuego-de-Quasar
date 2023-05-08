using Microsoft.AspNetCore.Mvc;
using operacion_fuego_quasar.Models;
using operacion_fuego_quasar.Services;

namespace operacion_fuego_quasar.Controllers
{
    /// <summary>
    /// Controlador general para la comunicacion con los satellites.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class topsecretController : ControllerBase
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
        /// <param name="data">Distancia y mensajes del satellite.</param>
        /// <returns>Respuesta con coordenadas y mensaje.</returns>
        /// <response code="200">Ok, Respuesta con coordenadas y mensaje.</response>
        /// <response code="400">Informacion de entrada invalida.</response>
        /// <response code="404">No se pudo determinar la posicion o el mensaje.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(responseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public IActionResult Post([FromBody] satellitesDTO data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Modelo invalido");
                }

                if (data.satellites?.Length < satelliteServices.coordenadasConocidas.Count)
                {
                    return BadRequest("Se requiere la distancia y mensaje de los 3 satelites conocidos.");
                }

                var listDistances = new List<double>();
                var listMessages = new List<List<string>>();
                foreach (var item in satelliteServices.coordenadasConocidas)
                {
                    var sat = data.satellites!.Where(w => w.name.ToLower() == item.Key.ToLower()).FirstOrDefault();
                    if (sat != null)
                    {
                        listDistances.Add(sat.distance ?? 0);
                        listMessages.Add(sat.message.ToList());
                    }
                    else
                    {
                        return BadRequest("Se requiere la distancia y mensaje de " + item.Key);
                    }
                }

                var coordenadas = satelliteServices.GetLocation(listDistances.ToArray());
                var mensaje = satelliteServices.GetMessage(listMessages);
                if ((coordenadas.y == null && coordenadas.y == null) || string.IsNullOrEmpty(mensaje))
                {
                    return NotFound("No se pudo determinar la posicion o el mensaje.");
                }
                return Ok(new responseDTO() { message = mensaje, position = coordenadas });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Metodo Delete para eliminar los datos guardados.
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="400">Error en la solicitud</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        public IActionResult Delete()
        {
            try
            {
                satelliteServices.ultimoMensaje = "";
                foreach (var item in satelliteServices.mensajesRecibidos)
                {
                    item.Value.Clear();
                }

                foreach (var item in satelliteServices.distanciasConocidas)
                {
                    satelliteServices.distanciasConocidas[item.Key] = null;
                }

                satelliteServices.coordenadasActuales.x = null;
                satelliteServices.coordenadasActuales.y = null;

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
