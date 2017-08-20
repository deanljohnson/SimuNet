using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimuNet.Tests
{
    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public void RegisterInitTest()
        {
            Register reg = new Register("A");
            Assert.AreEqual(reg.Name, "A");
            Assert.AreEqual(reg.Value, 0);
        }

        [TestMethod]
        public void RegisterValueTest()
        {
            Register reg = new Register("A");
            Assert.AreEqual(reg.Value, 0);
            reg.Value = 1;
            Assert.AreEqual(reg.Value, 1);
            reg.Value = -1;
            Assert.AreEqual(reg.Value, -1);
        }
    }
}