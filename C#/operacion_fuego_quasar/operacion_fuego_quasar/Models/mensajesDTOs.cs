using System.ComponentModel.DataAnnotations;

namespace operacion_fuego_quasar.Models
{
    /// <summary>
    /// Informacion de los satellites.
    /// </summary>
    public class satellitesDTO
    {
        /// <summary>
        /// Array de satellites.
        /// </summary>
        public satellite[] satellites { get; set; }
    }

    /// <summary>
    /// Objeto sattellite.
    /// </summary>
    public class satellite
    {
        /// <summary>
        /// Nombre del satellite.
        /// </summary>
        [Required]
        public string name { get; set; }

        /// <summary>
        /// Distancia del satellite hasta la nave.
        /// </summary>
        [Required]
        public double? distance { get; set; }

        /// <summary>
        /// Mensajes enviados del sattellite.
        /// </summary>
        [Required]
        public string[] message { get; set; }
    }

    /// <summary>
    /// Respuesta con la informacion procesada de la nave.
    /// </summary>
    public class responseDTO
    {
        public coordenadas position { get; set; }
        public string message { get; set; }
    }

    /// <summary>
    /// Informacion de entrada para un satellite.
    /// </summary>
    public class satelliteDTO
    {
        /// <summary>
        /// Distancia entre el satellite y la nave.
        /// </summary>
        [Required]
        public double? distance { get; set; }

        /// <summary>
        /// Mensajes enviados por el satellite.
        /// </summary>
        [Required]
        public string[] message { get; set; }
    }
}
