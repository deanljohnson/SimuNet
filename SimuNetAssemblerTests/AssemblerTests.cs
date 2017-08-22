using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimuNet;

namespace SimuNetAssembler.Tests
{
    [TestClass]
    public class AssemblerTests
    {
        [TestMethod]
        public void AssembleTest()
        {
            Assembler assem = new Assembler(new CPU());
            assem.Assemble(new FileInfo("Programs/successful.txt"));
        }
    }
}