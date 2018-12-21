using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimuNet;

namespace SimuNetTests
{
    [TestClass]
    public class ALUTests
    {
        [TestMethod]
        public void DoOpErrorTest()
        {
            ALU alu = new ALU();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => alu.DoOp(OpCode.Error, 0, 0, out int _));
        }

        [DataTestMethod]
        [DataRow(OpCode.NoOp, 1, 1, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [DataRow(OpCode.Add, 5, 7, 12, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Add, -5, -7, -12, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Add, -5, 7, 2, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Add, -5, 0, -5, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Add, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [DataRow(OpCode.Sub, 5, 7, -2, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Sub, -5, -7, 2, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Sub, -5, 7, -12, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Sub, -5, 0, -5, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Sub, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [DataRow(OpCode.Mul, 5, 7, 35, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Mul, 5, -7, -35, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Mul, -5, -7, 35, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Mul, 5, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]
        [DataRow(OpCode.Mul, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [DataRow(OpCode.Div, 10, 2, 5, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Div, -10, -2, 5, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.Div, -10, 2, -5, ALU.Flags.Negative, ALU.ErrorCode.None)]
        [DataRow(OpCode.Div, -10, 0, 0, ALU.Flags.None, ALU.ErrorCode.DivisionByZero)]
        [DataRow(OpCode.Div, 0, 0, 0, ALU.Flags.None, ALU.ErrorCode.DivisionByZero)]

        [DataRow(OpCode.LeftShift, 1, 1, 2, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.LeftShift, 1, 0, 1, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.LeftShift, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]

        [DataRow(OpCode.RightShift, 1, 1, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]
        [DataRow(OpCode.RightShift, 1, 0, 1, ALU.Flags.Positive, ALU.ErrorCode.None)]
        [DataRow(OpCode.RightShift, 0, 0, 0, ALU.Flags.Zero, ALU.ErrorCode.None)]
        public void DoOpTest(OpCode op, int a, int b, int expectedResult, ALU.Flags flags, ALU.ErrorCode error)
        {
            ALU alu = new ALU();
            alu.DoOp(op, a, b, out int actualResult);
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(flags, alu.StatusFlags);
            Assert.AreEqual(error, alu.Error);
        }
    }
}