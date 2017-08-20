using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimuNet.Tests
{
    [TestClass]
    public class InstructionTests
    {
        [TestMethod]
        public void InstructionInitTest()
        {
            Register aReg = new Register("A");
            Register bReg = new Register("B");
            Register cReg = new Register("C");

            Instruction instr = Instruction.Add(aReg, bReg, cReg);
            Assert.AreEqual(instr.Code, OpCode.Add);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.C, cReg);

            instr = Instruction.Sub(aReg, bReg, cReg);
            Assert.AreEqual(instr.Code, OpCode.Sub);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.C, cReg);

            instr = Instruction.Mul(aReg, bReg, cReg);
            Assert.AreEqual(instr.Code, OpCode.Mul);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.C, cReg);

            instr = Instruction.Div(aReg, bReg, cReg);
            Assert.AreEqual(instr.Code, OpCode.Div);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.C, cReg);

            instr = Instruction.Load(aReg, 10);
            Assert.AreEqual(instr.Code, OpCode.Load);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.Immediate1, 10);

            instr = Instruction.Equal(aReg, bReg, cReg);
            Assert.AreEqual(instr.Code, OpCode.Equal);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.C, cReg);

            instr = Instruction.Jump(5);
            Assert.AreEqual(instr.Code, OpCode.Jump);
            Assert.AreEqual(instr.Immediate1, 5);
            Assert.IsNull(instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.BranchOnZero(aReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnZero);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.BranchOnNotZero(aReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnNotZero);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.BranchOnEqual(aReg, bReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnEqual);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.NoOp();
            Assert.AreEqual(instr.Code, OpCode.NoOp);
            Assert.IsNull(instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.Error();
            Assert.AreEqual(instr.Code, OpCode.Error);
            Assert.IsNull(instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.Exit();
            Assert.AreEqual(instr.Code, OpCode.Exit);
            Assert.IsNull(instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);
        }
    }
}