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
            Assembler assem = new Assembler(new CPU());
            assem.Assemble(new FileInfo("Programs/successful.txt"));
        }

        [Fact]
        public void CommentsIgnored()
        {
            Assembler assem = new Assembler(new CPU());
            Program prog = assem.Assemble(new FileInfo("Programs/comments-ignored.txt"));
            Assert.Equal(3, prog.InstructionCount);
            Assert.Equal(OpCode.Move, prog[0].Code);
            Assert.Equal(OpCode.Add, prog[1].Code);
            Assert.Equal(OpCode.Exit, prog[2].Code);
        }

        [Theory]
        [InlineData("#begin noSymbol1 x y \n#end")]
        [InlineData("#begin noSymbol2 1 2 \n#end")]
        [InlineData("#begin notNumeric2 $1 $x \n#end")]
        [InlineData("#begin notNumeric3 $x $2 \n#end")]
        [InlineData("#begin wronglyOrdered $2 $1 \n#end")]
        [InlineData("#begin zero $0 \n#end")]
        public void Macros_BadArguments(string source)
        {
            byte[] sourceBytes = Encoding.ASCII.GetBytes(source);
            using (MemoryStream stream = new MemoryStream(sourceBytes))
            using (StreamReader reader = new StreamReader(stream))
            {
                Assembler assem = new Assembler(new CPU());
                Assert.Throws<InvalidOperationException>(() => assem.Assemble(reader));
            }
        }

        [Fact]
        public void Macro_SubstitutionOneArgTest()
        {
            string source =
                "#begin call $1\n" +
                "addi PC 2 RA\n" +
                "jump $1\n" +
                "#end\n" +
                "call method\n" +
                "exit\n" +
                "method:\n" +
                "print PC\n" +
                "exit";
            byte[] sourceBytes = Encoding.ASCII.GetBytes(source);
            using (MemoryStream stream = new MemoryStream(sourceBytes))
            using (StreamReader reader = new StreamReader(stream))
            {
                Assembler assem = new Assembler(new CPU());
                Program program = assem.Assemble(reader);
                Assert.Equal(OpCode.AddI, program[0].Code);
                Assert.Equal(OpCode.Jump, program[1].Code);
                Assert.Equal(OpCode.Exit, program[2].Code);
                Assert.Equal(OpCode.NoOp, program[3].Code);
                Assert.Equal(OpCode.PrintRegister, program[4].Code);
                Assert.Equal(OpCode.Exit, program[5].Code);
            }
        }

        [Fact]
        public void Macro_SubstitutionTwoArgTest()
        {
            string source =
                "#begin call $1 $2\n" +
                "addi PC $2 RA\n" +
                "jump $1\n" +
                "#end\n" +
                "call method 2\n" +
                "exit\n" +
                "method:\n" +
                "print PC\n" +
                "exit";
            byte[] sourceBytes = Encoding.ASCII.GetBytes(source);
            using (MemoryStream stream = new MemoryStream(sourceBytes))
            using (StreamReader reader = new StreamReader(stream))
            {
                Assembler assem = new Assembler(new CPU());
                Program program = assem.Assemble(reader);
                Assert.Equal(OpCode.AddI, program[0].Code);
                Assert.Equal(OpCode.Jump, program[1].Code);
                Assert.Equal(OpCode.Exit, program[2].Code);
                Assert.Equal(OpCode.NoOp, program[3].Code);
                Assert.Equal(OpCode.PrintRegister, program[4].Code);
                Assert.Equal(OpCode.Exit, program[5].Code);
            }
        }
    }
}