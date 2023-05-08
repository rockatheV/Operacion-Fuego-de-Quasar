using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace operacion_fuego_quasar.Services.Tests
{
    [TestClass()]
    public class satelliteServicesTests
    {
        [TestMethod()]
        public void GetLocationTest()
        {
            var result = satelliteServices.GetLocation(new[] { 412.31, 282.84, 721.11 });
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.x);
            Assert.IsNotNull(result.y);
            Assert.AreEqual(Math.Round(result.x!.Value), -100);
            Assert.AreEqual(Math.Round(result.y!.Value), -300);
        }

        [TestMethod()]
        public void GetMessageTest()
        {
            var result = satelliteServices.GetMessage(
                new List<List<string>>() { 
                    new List<string>() { "", "este", "es", "un", "mensaje" },
                    new List<string>() { "este", "", "un", "mensaje" },
                    new List<string>() { "", "", "es", "", "mensaje" }
                });
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, "");
            Assert.AreEqual("este es un mensaje", result);

            result = satelliteServices.GetMessage(
                new List<List<string>>() {
                    new List<string>() { "", "este", "es", "un", "mensaje" },
                    new List<string>() { "este", "", "un", "mensaje", "secreto" },
                    new List<string>() { "", "", "es", "", "mensaje" }
                });
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, "");
            Assert.AreEqual("este es un mensaje secreto", result);
        }
    }
}