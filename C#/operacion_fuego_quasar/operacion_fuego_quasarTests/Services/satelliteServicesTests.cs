using Microsoft.VisualStudio.TestTools.UnitTesting;
using operacion_fuego_quasar.Models;

namespace operacion_fuego_quasar.Services.Tests
{
    [TestClass()]
    public class satelliteServicesTests
    {
        [DataRow(new[] { 412.31, 282.84, 721.11 }, -100, -300)]
        [DataRow(new[] { 1004.99, 447.21, 400 }, 500, -300)]
        [TestMethod()]
        public void GetLocationTest(double[] distancias, double x, double y)
        {
            var result = satelliteServices.GetLocation(distancias);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.x);
            Assert.IsNotNull(result.y);
            Assert.AreEqual(Math.Round(result.x!.Value), x);
            Assert.AreEqual(Math.Round(result.y!.Value), y);
        }

        [DataRow(
            new string[] { "", "este", "es", "un", "mensaje" },
            new string[] { "este", "", "un", "mensaje" },
            new string[] { "", "", "es", "", "mensaje" },
            "este es un mensaje"
            )]
        [DataRow(
            new string[] { "", "este", "es", "un", "mensaje" },
            new string[] { "este", "", "un", "mensaje", "secreto" },
            new string[] { "", "", "es", "", "mensaje" },
            "este es un mensaje secreto"
            )]
        [TestMethod()]
        public void GetMessageTest(string[] msg1, string[] msg2, string[] msg3, string response)
        {
            var result = satelliteServices.GetMessage(
                new List<List<string>>() { 
                    msg1.ToList(),
                    msg2.ToList(),
                    msg3.ToList()
                });
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, "");
            Assert.AreEqual(response, result);
        }
    }
}