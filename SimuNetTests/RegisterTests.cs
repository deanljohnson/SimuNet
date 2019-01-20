using SimuNet;
using Xunit;

namespace SimuNetTests
{
    public class RegisterTests
    {
        [Fact]
        public void RegisterInitTest()
        {
            Register reg = new Register("A");
            Assert.Equal("A", reg.Name);
            Assert.Equal(0, reg.Value);
        }

        [Fact]
        public void RegisterValueTest()
        {
            Register reg = new Register("A");
            Assert.Equal(0, reg.Value);
            reg.Value = 1;
            Assert.Equal(1, reg.Value);
            reg.Value = -1;
            Assert.Equal(-1, reg.Value);
        }
    }
}