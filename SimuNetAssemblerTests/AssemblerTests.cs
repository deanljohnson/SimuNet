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
            assem.BeginProgram();
            assem.Assemble(new FileInfo("Programs/comments-ignored.txt"));
            Program prog = assem.EndProgram();
            Assert.Equal(3, prog.InstructionCount);
            Assert.Equal(OpCode.Move, prog[0].Code);
            Assert.Equal(OpCode.Add, prog[1].Code);
            Assert.Equal(OpCode.Exit, prog[2].Code);
        }

        [Fact]
        public void MultiFileTest()
        {
            CPU cpu = new CPU(new Memory(65536));
            Assembler assem = new Assembler(cpu);
            assem.BeginProgram();

            assem.Assemble(new FileInfo("Programs/multi-file-1.txt"));
            assem.Assemble(new FileInfo("Programs/multi-file-2.txt"));

            Program prog = assem.EndProgram();
            Assert.Equal(5, prog.InstructionCount);
            Assert.Equal(OpCode.LoadI, prog[0].Code);
            Assert.Equal(OpCode.Jump, prog[1].Code);
            Assert.Equal(OpCode.Exit, prog[2].Code);
            Assert.Equal(OpCode.Add, prog[3].Code);
            Assert.Equal(OpCode.Exit, prog[4].Code);

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.Equal(10, cpu.V0.Value);
        }
    }
}