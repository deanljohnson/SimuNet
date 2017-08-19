using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimuNet.Tests
{
    [TestClass]
    public class ALUTests
    {
        [TestMethod]
        public void DoOpTest()
        {
            ALU alu = new ALU();

            alu.DoOp(ALU.OpCode.AddI, 5, 7, out int addPosResult);
            Assert.AreEqual(addPosResult, 12);
            alu.DoOp(ALU.OpCode.AddI, -5, -7, out int addNegResult);
            Assert.AreEqual(addNegResult, -12);
            alu.DoOp(ALU.OpCode.AddI, -5, 7, out int addMixedResult);
            Assert.AreEqual(addMixedResult, 2);
            alu.DoOp(ALU.OpCode.AddI, -5, 0, out int addZeroResult);
            Assert.AreEqual(addZeroResult, -5);
            alu.DoOp(ALU.OpCode.AddI, 0, 0, out int addNoneResult);
            Assert.AreEqual(addNoneResult, 0);

            alu.DoOp(ALU.OpCode.SubI, 3, 10, out int subPosResult);
            Assert.AreEqual(subPosResult, -7);
            alu.DoOp(ALU.OpCode.SubI, -3, -10, out int subNegResult);
            Assert.AreEqual(subNegResult, 7);
            alu.DoOp(ALU.OpCode.SubI, -3, 10, out int subMixedResult);
            Assert.AreEqual(subMixedResult, -13);
            alu.DoOp(ALU.OpCode.SubI, -5, 0, out int subZeroResult);
            Assert.AreEqual(subZeroResult, -5);
            alu.DoOp(ALU.OpCode.SubI, 0, 0, out int subNoneResult);
            Assert.AreEqual(subNoneResult, 0);

            alu.DoOp(ALU.OpCode.MulI, 5, 7, out int mulPosResult);
            Assert.AreEqual(mulPosResult, 35);
            alu.DoOp(ALU.OpCode.MulI, -5, -7, out int mulNegResult);
            Assert.AreEqual(mulNegResult, 35);
            alu.DoOp(ALU.OpCode.MulI, -5, 7, out int mulMixedResult);
            Assert.AreEqual(mulMixedResult, -35);
            alu.DoOp(ALU.OpCode.MulI, -5, 0, out int mulZeroResult);
            Assert.AreEqual(mulZeroResult, 0);
            alu.DoOp(ALU.OpCode.MulI, 0, 0, out int mulNoneResult);
            Assert.AreEqual(mulNoneResult, 0);
            
            alu.DoOp(ALU.OpCode.DivI, 10, 2, out int divPosResult);
            Assert.AreEqual(divPosResult, 5);
            alu.DoOp(ALU.OpCode.DivI, -10, -2, out int divNegResult);
            Assert.AreEqual(divNegResult, 5);
            alu.DoOp(ALU.OpCode.DivI, -10, 5, out int divMixedResult);
            Assert.AreEqual(divMixedResult, -2);
            alu.DoOp(ALU.OpCode.DivI, -5, 0, out int divZeroResult);
            Assert.AreEqual(divZeroResult, 0);
            Assert.AreEqual(alu.Error, ALU.ErrorCode.DivisionByZero);
            alu.DoOp(ALU.OpCode.DivI, 0, 0, out int divNoneResult);
            Assert.AreEqual(divNoneResult, 0);
            Assert.AreEqual(alu.Error, ALU.ErrorCode.DivisionByZero);
        }
    }
}