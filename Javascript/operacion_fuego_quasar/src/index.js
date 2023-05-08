import { GetLocation, GetMessage, coordenadasActuales, ultimoMensaje, mensajesRecibidos, distanciasConocidas } from './app.js';
import express from 'express';
import bodyParser from 'body-parser';

console.log('Iniciando app...');

const app = express();
const port = 3000;

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

app.get('/', (req, res) => {
    res.send('Bienvenido a Operacion fuego Quasar!!!');
});

app.get('/topsecret', (req, res) => {
    if ((coordenadasActuales.x === null && coordenadasActuales.y === null) || ultimoMensaje === '') {
        res.status(404).json();
        return;
    }
    res.status(200).json({ position: coordenadasActuales, mensaje: ultimoMensaje });
});

app.get('/topsecret_split', (req, res) => {
    if ((coordenadasActuales.x === null && coordenadasActuales.y === null) || ultimoMensaje === '') {
        res.status(404).json();
        return;
    }
    res.status(200).json({ position: coordenadasActuales, mensaje: ultimoMensaje });
});

app.post('/topsecret', (req, res) => {
    let data = req.body;
    if (!data.satellites || data.satellites.length < 3) {
        res.status(400).json({ message: 'Modelo invalido'});
        return;
    }
    let distances = [];
    let messages = [];
    data.satellites.forEach(element => {
        if (!element.name || !element.distance || !element.message || element.message?.length === 0) {
            res.status(400).json({ message: 'Modelo invalido'});
            return;
        }
        distances.push(element.distance);
        messages.push(element.message);
    });
    let coordenadas = GetLocation(distances);
    let message = GetMessage(messages);
    res.status(200).json({ position: coordenadas, message: message });
});

app.post('/topsecret_split/:satellite_name', (req, res) => {
    let data = req.body;
    let satellite_name = req.params.satellite_name;
    if (!data.distance || !data.message || data.message?.length === 0) {
        res.status(400).json({ message: 'Modelo invalido'});
        return;
    }
    let distances = [];
    
    distanciasConocidas[satellite_name] = data.distance;
    Object.values(distanciasConocidas).forEach(item => {
        distances.push(item);
    });
    let coordenadas = GetLocation(distances);

    mensajesRecibidos[satellite_name] = data.message;
    GetMessage(Object.values(mensajesRecibidos));
    if ((coordenadasActuales.x === null && coordenadasActuales.y === null) || ultimoMensaje === '') {
        res.status(404).json();
        return;
    }
    res.status(200).json({ position: coordenadas, message: ultimoMensaje });
});

app.listen(port, () => {
    console.log(`Aplicacion corriendo en el puerto ${port}`);
});
