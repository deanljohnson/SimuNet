using System;
using System.IO;
using System.Text;
using SimuNet;
using Xunit;

namespace SimuNetAssembler.Tests
{
    public class AssemblerTests
    {
        [Fact]
        public void AssembleTest()
        {
            // Assemble a file with all supported instructions.
            // Just testing for exceptions at this point.
            Assembler assem = new Assembler(new CPU(new Memory(65536)));
            assem.Assemble(new FileInfo("Programs/successful.txt"));
        }

        [Fact]
        public void CommentsIgnored()
        {
            Assembler assem = new Assembler(new CPU(new Memory(65536)));
            Program prog = assem.Assemble(new FileInfo("Programs/comments-ignored.txt"));
            Assert.Equal(3, prog.InstructionCount);
            Assert.Equal(OpCode.Move, prog[0].Code);
            Assert.Equal(OpCode.Add, prog[1].Code);
            Assert.Equal(OpCode.Exit, prog[2].Code);
        }
    }
}