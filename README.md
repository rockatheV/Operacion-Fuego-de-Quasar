# Operacion-Fuego-de-Quasar
El objetivo es un programa que retorne la fuente y contenido del mensaje de auxilio.
Para esto, se contó con tres satélites que permiten triangular la posición, prevee que el
mensaje puede no llegar completo a cada satélite debido al campo de asteroides.

Posición de los satélites actualmente en servicio:
- Kenobi: [-500, -200]
- Skywalker: [100, -100]
- Sato: [500, 100]

El programa recibe la información de distancias al emisor y los mensajes en cada
satélite retornando el mensaje traducido y completo con las coordenadas de la nave.
El programa permite el envío de la información de forma completa como parcial por
cada satélite, además esta información es guardada y puede ser consultada a el
programa en todo momento.

## Información del programa
Desarrollado en C# (.NET 6) con Visual Studio Community, implementado en el cloud
de Azure, se entrega información especifica de la API de forma separada a este
documento. El programa incluye documentación a nivel de código y pruebas unitarias
de los servicios. Se contempla también una versión desarrollada en Javascript (NodeJs)
no implementada pero funcional e implementable.
