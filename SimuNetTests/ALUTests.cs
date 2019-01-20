using System;
using SimuNet;
using Xunit;

namespace SimuNetTests
{
    public class ALUTests
    {
        [Fact]
        public void DoOpErrorTest()
        {
            ALU alu = new ALU();
            Assert.Throws<ArgumentOutOfRangeException>(() => alu.DoOp(OpCode.Error, 0, 0, out int _, out int _));
        }

        [Theory]
        [InlineData(OpCode.NoOp, 1, 1, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [InlineData(OpCode.Add, 5, 7, 12, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Add, -5, -7, -12, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Add, -5, 7, 2, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Add, -5, 0, -5, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Add, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [InlineData(OpCode.Sub, 5, 7, -2, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Sub, -5, -7, 2, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Sub, -5, 7, -12, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Sub, -5, 0, -5, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Sub, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [InlineData(OpCode.Mul, 5, 7, 35, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Mul, 5, -7, -35, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Mul, -5, -7, 35, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Mul, 5, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]
        [InlineData(OpCode.Mul, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [InlineData(OpCode.Div, 10, 2, 5, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Div, -10, -2, 5, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.Div, -10, 2, -5, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [InlineData(OpCode.Div, -10, 0, 0, ALU.Flags.None, ALU.ErrorCode.DivisionByZero)]
        [InlineData(OpCode.Div, 0, 0, 0, ALU.Flags.None, ALU.ErrorCode.DivisionByZero)]

        [InlineData(OpCode.LeftShift, 1, 1, 2, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.LeftShift, 1, 0, 1, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.LeftShift, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [InlineData(OpCode.RightShift, 1, 1, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]
        [InlineData(OpCode.RightShift, 1, 0, 1, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [InlineData(OpCode.RightShift, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]
        public void DoOpTest(OpCode op, int a, int b, int expectedResult, ALU.Flags flags, ALU.ErrorCode error)
        {
            ALU alu = new ALU();
            alu.DoOp(op, a, b, out int actualResult, out int _);
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(flags, alu.StatusFlags);
            Assert.Equal(error, alu.Error);
        }
    }
}