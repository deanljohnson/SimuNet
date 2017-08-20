using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimuNet.Tests
{
    [TestClass]
    public class ALUTests
    {
        [TestMethod]
        public void DoOpIntTest()
        {
            ALU alu = new ALU();

            alu.DoOp(OpCode.Add, 5, 7, out int addPosResult);
            Assert.AreEqual(addPosResult, 12);
            alu.DoOp(OpCode.Add, -5, -7, out int addNegResult);
            Assert.AreEqual(addNegResult, -12);
            alu.DoOp(OpCode.Add, -5, 7, out int addMixedResult);
            Assert.AreEqual(addMixedResult, 2);
            alu.DoOp(OpCode.Add, -5, 0, out int addZeroResult);
            Assert.AreEqual(addZeroResult, -5);
            alu.DoOp(OpCode.Add, 0, 0, out int addNoneResult);
            Assert.AreEqual(addNoneResult, 0);

            alu.DoOp(OpCode.Sub, 3, 10, out int subPosResult);
            Assert.AreEqual(subPosResult, -7);
            alu.DoOp(OpCode.Sub, -3, -10, out int subNegResult);
            Assert.AreEqual(subNegResult, 7);
            alu.DoOp(OpCode.Sub, -3, 10, out int subMixedResult);
            Assert.AreEqual(subMixedResult, -13);
            alu.DoOp(OpCode.Sub, -5, 0, out int subZeroResult);
            Assert.AreEqual(subZeroResult, -5);
            alu.DoOp(OpCode.Sub, 0, 0, out int subNoneResult);
            Assert.AreEqual(subNoneResult, 0);

            alu.DoOp(OpCode.Mul, 5, 7, out int mulPosResult);
            Assert.AreEqual(mulPosResult, 35);
            alu.DoOp(OpCode.Mul, -5, -7, out int mulNegResult);
            Assert.AreEqual(mulNegResult, 35);
            alu.DoOp(OpCode.Mul, -5, 7, out int mulMixedResult);
            Assert.AreEqual(mulMixedResult, -35);
            alu.DoOp(OpCode.Mul, -5, 0, out int mulZeroResult);
            Assert.AreEqual(mulZeroResult, 0);
            alu.DoOp(OpCode.Mul, 0, 0, out int mulNoneResult);
            Assert.AreEqual(mulNoneResult, 0);
            
            alu.DoOp(OpCode.Div, 10, 2, out int divPosResult);
            Assert.AreEqual(divPosResult, 5);
            alu.DoOp(OpCode.Div, -10, -2, out int divNegResult);
            Assert.AreEqual(divNegResult, 5);
            alu.DoOp(OpCode.Div, -10, 5, out int divMixedResult);
            Assert.AreEqual(divMixedResult, -2);
            alu.DoOp(OpCode.Div, -5, 0, out int divZeroResult);
            Assert.AreEqual(divZeroResult, 0);
            Assert.AreEqual(alu.Error, ALU.ErrorCode.DivisionByZero);
            alu.DoOp(OpCode.Div, 0, 0, out int divNoneResult);
            Assert.AreEqual(divNoneResult, 0);
            Assert.AreEqual(alu.Error, ALU.ErrorCode.DivisionByZero);

            alu.DoOp(OpCode.Equal, 5, 5, out int trueEqualResult);
            Assert.AreEqual(trueEqualResult, 1);
            alu.DoOp(OpCode.Equal, 0, 5, out int falseEqualResult);
            Assert.AreEqual(falseEqualResult, 0);

            alu.DoOp(OpCode.NoOp, 1, 1, out int noOpResult);
            Assert.AreEqual(noOpResult, 0);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => alu.DoOp(OpCode.Error, 0, 0, out int _));
        }
    }
}