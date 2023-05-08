/**
 * Posicion de los satelites actualmente en servicio.
 * @type {posiciones}
 */
const coordenadasConocidas = {
    Kenobi: { x: -500, y: -200 },
    Skywalker: { x: 100, y: -100 },
    Sato: { x: 500, y: 100}
};

/**
 * Coordenadas guardadas de la nave.
 * @type {coordenadas}
 */
export let coordenadasActuales = {
    x: null,
    y: null
};

/**
 * Distancias guardadas a los satelites en servicio.
 * @type {distancias}
 */
export let distanciasConocidas = {
    Kenobi: null,
    Skywalker: null,
    Sato: null
};

/**
 * Mensajes recibidos de los satelites en servicio.
 * @type {mensajes}
 */
export let mensajesRecibidos = {
    Kenobi: [],
    Skywalker: [],
    Sato: []
};

export let ultimoMensaje = '';

/**
 * Metodo para calcular las coordenadas de acuerdo a las distancias.
 * @param {number[]} distances distancia al emisor tal cual se recibe en cada satelite.
 * @returns {any} coordenadas 'x' e 'y' del emisor del mensaje.
 */
export function GetLocation(distances) {
    if (distances.length < 3) {
        coordenadasActuales = { x: null, y: null };
        return coordenadasActuales;
    }

    distanciasConocidas.Kenobi = distances[0];
    distanciasConocidas.Skywalker = distances[1];
    distanciasConocidas.Sato = distances[2];

    let salir = false;
    distances.forEach(item => {
        if (!item) {
            salir = true;
        }
    });
    if (salir) return coordenadasActuales;

    const A = (-2 * coordenadasConocidas.Kenobi.x) + (2 * coordenadasConocidas.Skywalker.x);
    const B = (-2 * coordenadasConocidas.Kenobi.y) + (2 * coordenadasConocidas.Skywalker.y);
    const R1 = distanciasConocidas.Kenobi * distanciasConocidas.Kenobi - distanciasConocidas.Skywalker * distanciasConocidas.Skywalker - coordenadasConocidas.Kenobi.x * coordenadasConocidas.Kenobi.x + coordenadasConocidas.Skywalker.x * coordenadasConocidas.Skywalker.x - coordenadasConocidas.Kenobi.y * coordenadasConocidas.Kenobi.y + coordenadasConocidas.Skywalker.y * coordenadasConocidas.Skywalker.y;
    
    const C = (-2 * coordenadasConocidas.Skywalker.x) + (2 * coordenadasConocidas.Sato.x);
    const D = (-2 * coordenadasConocidas.Skywalker.y) + (2 * coordenadasConocidas.Sato.y);
    const R2 = distanciasConocidas.Skywalker * distanciasConocidas.Skywalker - distanciasConocidas.Sato * distanciasConocidas.Sato - coordenadasConocidas.Skywalker.x * coordenadasConocidas.Skywalker.x + coordenadasConocidas.Sato.x * coordenadasConocidas.Sato.x - coordenadasConocidas.Skywalker.y * coordenadasConocidas.Skywalker.y + coordenadasConocidas.Sato.y * coordenadasConocidas.Sato.y;
    
    let x = (R1 * D - R2 * B) / (D * A - B * C);
    let y = (R1 * C - A * R2) / (B * C - A * D);

    x = parseFloat(x.toFixed(2));
    y = parseFloat(y.toFixed(2));

    coordenadasActuales = { x, y };
    return coordenadasActuales;
}

/**
 * Metodos para la interpretacion de mensajes.
 * @param {string[][]} messages el mensaje tal cual es recibido en cada satelite.
 * @returns {string} el mensaje tal cual lo genera el emisor del mensaje.
 */
export function GetMessage(messages) {
    messages.forEach((msjs, index) => {
        mensajesRecibidos[Object.keys(mensajesRecibidos)[index]] = msjs;
    });

    const parsedMensajes = new Set();
    messages.forEach(messageList => {
        messageList.forEach(mensage => {
            if (mensage !== '' && !parsedMensajes.has(mensage)) {
                parsedMensajes.add(mensage);
            }
        });
    });
    ultimoMensaje = [...parsedMensajes].join(' ');
    return ultimoMensaje;
}
