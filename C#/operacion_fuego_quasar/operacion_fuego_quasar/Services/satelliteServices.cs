using operacion_fuego_quasar.Models;

namespace operacion_fuego_quasar.Services
{
    /// <summary>
    /// Servicio para el control de informacion de los satellites.
    /// </summary>
    public static class satelliteServices
    {
        #region Atributos
        /// <summary>
        /// Coordenadas conocidas de los satellites.
        /// </summary>
        public static Dictionary<string, coordenadas> coordenadasConocidas =
            new Dictionary<string, coordenadas>() {
                {"Kenobi", new coordenadas() { x = -500, y = -200 } },
                {"Skywalker", new coordenadas() { x = 100, y = -100 } },
                {"Sato", new coordenadas() { x = 500, y = 100 } }
            };

        /// <summary>
        /// Coordenadas actuales de la nave.
        /// </summary>
        public static coordenadas coordenadasActuales =
            new coordenadas() { x = null, y = null };

        /// <summary>
        /// Distancias conocidas actuales de los satellites.
        /// </summary>
        public static Dictionary<string, double?> distanciasConocidas =
            new Dictionary<string, double?>() {
                { "Kenobi", null },
                { "Skywalker", null },
                { "Sato", null }
            };

        /// <summary>
        /// Mensajes recibidos de los satellites.
        /// </summary>
        public static Dictionary<string, List<string>> mensajesRecibidos =
            new Dictionary<string, List<string>>() {
                {"Kenobi", new List<string>()},
                {"Skywalker", new List<string>()},
                {"Sato", new List<string>()}
            };

        /// <summary>
        /// Ultimo mensaje guardado.
        /// </summary>
        public static string ultimoMensaje = string.Empty;
        #endregion

        #region Metodos
        /// <summary>
        /// Metodo para calcular las coordenadas de acuerdo a las distancias.
        /// </summary>
        /// <param name="distances"></param>
        /// <returns>Coordenadas actuales calculadas</returns>
        public static coordenadas GetLocation(double[] distances)
        {
            // Validaciones
            if (distances.Length < 3)
            {
                coordenadasActuales.x = null;
                coordenadasActuales.y = null;
                return coordenadasActuales;
            }

            try
            {
                // Guardado de datos.
                var dNave1 = distanciasConocidas[distanciasConocidas.ElementAt(0).Key] = distances[0];
                var dNave2 = distanciasConocidas[distanciasConocidas.ElementAt(1).Key] = distances[1];
                var dNave3 = distanciasConocidas[distanciasConocidas.ElementAt(2).Key] = distances[2];

                foreach (var item in distanciasConocidas)
                {
                    if (item.Value == null)
                    {
                        return coordenadasActuales;
                    }
                }

                var cNave1 = coordenadasConocidas[coordenadasConocidas.ElementAt(0).Key];
                var cNave2 = coordenadasConocidas[coordenadasConocidas.ElementAt(1).Key];
                var cNave3 = coordenadasConocidas[coordenadasConocidas.ElementAt(2).Key];

                // Calculo de posicion a partir de la informacion obtenida.
                double A = (-2 * cNave1.x!.Value) + (2 * cNave2.x!.Value);
                double B = (-2 * cNave1.y!.Value) + (2 * cNave2.y)!.Value;
                double R1 = dNave1!.Value * dNave1!.Value - dNave2!.Value * dNave2!.Value - cNave1.x!.Value * cNave1.x!.Value + cNave2.x!.Value * cNave2.x!.Value - cNave1.y!.Value * cNave1.y!.Value + cNave2.y!.Value * cNave2.y!.Value;

                double C = (-2 * cNave2.x!.Value) + (2 * cNave3.x!.Value);
                double D = (-2 * cNave2.y!.Value) + (2 * cNave3.y!.Value);
                double R2 = dNave2!.Value * dNave2!.Value - dNave3!.Value * dNave3!.Value - cNave2.x!.Value * cNave2.x!.Value + cNave3.x!.Value * cNave3.x!.Value - cNave2.y!.Value * cNave2.y!.Value + cNave3.y!.Value * cNave3.y!.Value;

                double x = (R1 * D - R2 * B) / (D * A - B * C);
                double y = (R1 * C - A * R2) / (B * C - A * D);

                x = Math.Round(x, 2);
                y = Math.Round(y, 2);

                // Respuesta.
                coordenadasActuales.x = x;
                coordenadasActuales.y = y;
                return coordenadasActuales;
            }
            catch (Exception ex)
            {
                return new coordenadas() { x = null, y = null };
            }
        }

        /// <summary>
        /// Metodos para la interpretacion de mensajes.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns>Mensaje final armado a partir de la lista de mensajes.</returns>
        public static string GetMessage(List<List<string>> messages)
        {
            try
            {
                // Guardado de datos.
                int index = 0;
                foreach (var item in mensajesRecibidos.Keys.ToList())
                {
                    mensajesRecibidos[mensajesRecibidos.Keys.ToList()[index]] = messages[index];
                    index++;
                }

                // Armado del mensaje.
                HashSet<string> parsedMensajes = new HashSet<string>();
                foreach (List<string> messageList in messages)
                {
                    foreach (string mensage in messageList)
                    {
                        if (!string.IsNullOrEmpty(mensage) && !parsedMensajes.Contains(mensage))
                        {
                            parsedMensajes.Add(mensage);
                        }
                    }
                }

                // Respuesta
                ultimoMensaje = string.Join(" ", parsedMensajes);
                return ultimoMensaje;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
    }
}
