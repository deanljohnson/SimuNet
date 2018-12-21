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

        [TestMethod]
        public void CommentsIgnored()
        {
            Assembler assem = new Assembler(new CPU());
            Program prog = assem.Assemble(new FileInfo("Programs/comments-ignored.txt"));
            Assert.AreEqual(3, prog.InstructionCount);
            Assert.AreEqual(OpCode.Move, prog[0].Code);
            Assert.AreEqual(OpCode.Add, prog[1].Code);
            Assert.AreEqual(OpCode.Exit, prog[2].Code);
        }
    }
}