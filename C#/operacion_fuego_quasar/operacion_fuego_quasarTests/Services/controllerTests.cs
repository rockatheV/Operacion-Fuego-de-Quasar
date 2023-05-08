using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using operacion_fuego_quasar.Controllers;
using operacion_fuego_quasar.Models;

namespace operacion_fuego_quasar.Services.Tests
{
    [TestClass()]
    public class controllerTests
    {
        [TestMethod()]
        public void topsecretTest()
        {
            var controller = new topsecretController();
            var result404 = controller.Get() as NotFoundObjectResult;
            Assert.IsNotNull(result404);
            Assert.AreEqual(StatusCodes.Status404NotFound, result404.StatusCode);

            var satelites = new satellitesDTO()
            {
                satellites = new satellite[]
                {
                    new satellite() { name = "Kenobi", distance = 412.31, message = new string[] { "", "este", "es", "un", "mensaje" } },
                    new satellite() { name = "Skywalker", distance = 282.84, message = new string[] { "este", "", "un", "mensaje" } },
                    new satellite() { name = "Sato", distance = 721.11, message = new string[] { "", "", "es", "", "mensaje" } }
                }
            };
            var resultPost200 = controller.Post(satelites) as OkObjectResult;
            Assert.IsNotNull(resultPost200);
            Assert.AreEqual(StatusCodes.Status200OK, resultPost200.StatusCode);
            var value = resultPost200.Value as responseDTO;
            Assert.IsNotNull(value);
            Assert.AreEqual(-300.01, value.position.y);
            Assert.AreEqual(-100, value.position.x);
            Assert.AreEqual("este es un mensaje", value.message);

            var result200 = controller.Get() as OkObjectResult;
            Assert.IsNotNull(result200);
            Assert.AreEqual(StatusCodes.Status200OK, result200.StatusCode);
            value = result200.Value as responseDTO;
            Assert.IsNotNull(value);
            Assert.AreEqual(-300.01, value.position.y);
            Assert.AreEqual(-100, value.position.x);
            Assert.AreEqual("este es un mensaje", value.message);

            satelites = new satellitesDTO()
            {
                satellites = new satellite[]
                {
                    new satellite() { name = "Kenobi", distance = 412.31, message = new string[] { "", "este", "es", "un", "mensaje" } },
                    new satellite() { name = "Sato", distance = 721.11, message = new string[] { "", "", "es", "", "mensaje" } }
                }
            };
            var resultPost400 = controller.Post(satelites) as BadRequestObjectResult;
            Assert.IsNotNull(resultPost400);
            Assert.AreEqual(StatusCodes.Status400BadRequest, resultPost400.StatusCode);
        }

        [TestMethod()]
        public void topsecret_splitTest()
        {
            var controller = new topsecret_splitController();
            var result404 = controller.Get() as NotFoundObjectResult;
            Assert.IsNotNull(result404);
            Assert.AreEqual(StatusCodes.Status404NotFound, result404.StatusCode);

            var satellites = new satellite[]
            {
                new satellite() { name = "Kenobi", distance = 412.31, message = new string[] { "", "este", "es", "un", "mensaje" } },
                new satellite() { name = "Skywalker", distance = 282.84, message = new string[] { "este", "", "un", "mensaje" } },
                new satellite() { name = "Sato", distance = 721.11, message = new string[] { "", "", "es", "", "mensaje" } }
            };

            var index = 0;
            foreach (var item in satellites)
            {
                if (index == 2)
                {
                    var resultPost = controller.Post(
                    item.name,
                    new satelliteDTO() { distance = item.distance, message = item.message }
                    ) as OkObjectResult;
                    Assert.IsNotNull(resultPost);
                    Assert.AreEqual(StatusCodes.Status200OK, resultPost.StatusCode);
                    var value = resultPost.Value as responseDTO;
                    Assert.IsNotNull(value);
                    Assert.AreEqual(-300.01, value.position.y);
                    Assert.AreEqual(-100, value.position.x);
                    Assert.AreEqual("este es un mensaje", value.message);
                }
                else
                {
                    var resultPost = controller.Post(
                    item.name,
                    new satelliteDTO() { distance = item.distance, message = item.message }
                    ) as NotFoundObjectResult;
                    Assert.IsNotNull(resultPost);
                    Assert.AreEqual(StatusCodes.Status404NotFound, resultPost.StatusCode);
                }
                index++;
            }

            var result200 = controller.Get() as OkObjectResult;
            Assert.IsNotNull(result200);
            Assert.AreEqual(StatusCodes.Status200OK, result200.StatusCode);
        }
    }
}
