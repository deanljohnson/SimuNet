using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimuNet;

namespace SimuNetTests
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

            instr = Instruction.AddI(aReg, 5, bReg);
            Assert.AreEqual(instr.Code, OpCode.AddI);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.Immediate1, 5);

            instr = Instruction.SubI(aReg, 5, bReg);
            Assert.AreEqual(instr.Code, OpCode.SubI);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.Immediate1, 5);

            instr = Instruction.MulI(aReg, 5,  bReg);
            Assert.AreEqual(instr.Code, OpCode.MulI);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.Immediate1, 5);

            instr = Instruction.DivI(aReg, 5, bReg);
            Assert.AreEqual(instr.Code, OpCode.DivI);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.Immediate1, 5);

            instr = Instruction.Load(aReg, 10);
            Assert.AreEqual(instr.Code, OpCode.Load);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.Immediate1, 10);

            instr = Instruction.Move(aReg, bReg);
            Assert.AreEqual(instr.Code, OpCode.Move);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.Equal(aReg, bReg, cReg);
            Assert.AreEqual(instr.Code, OpCode.Equal);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);
            Assert.AreEqual(instr.C, cReg);

            instr = Instruction.Push(aReg, 10);
            Assert.AreEqual(instr.Code, OpCode.Push);
            Assert.AreEqual(instr.Immediate1, 10);
            Assert.AreEqual(aReg, instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.Pop(aReg, 10);
            Assert.AreEqual(instr.Code, OpCode.Pop);
            Assert.AreEqual(instr.Immediate1, 10);
            Assert.AreEqual(aReg, instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.Jump(5);
            Assert.AreEqual(instr.Code, OpCode.Jump);
            Assert.AreEqual(instr.Immediate1, 5);
            Assert.IsNull(instr.A);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

            instr = Instruction.JumpRegister(aReg);
            Assert.AreEqual(instr.Code, OpCode.JumpRegister);
            Assert.AreEqual(instr.A, aReg);
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

            instr = Instruction.BranchOnNotEqual(aReg, bReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnNotEqual);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.BranchOnLessThan(aReg, bReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnLessThan);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.BranchOnGreaterThan(aReg, bReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnGreaterThan);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.BranchOnLessThanOrEqual(aReg, bReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnLessThanOrEqual);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.BranchOnGreaterThanOrEqual(aReg, bReg, 7);
            Assert.AreEqual(instr.Code, OpCode.BranchOnGreaterThanOrEqual);
            Assert.AreEqual(instr.Immediate1, 7);
            Assert.AreEqual(instr.A, aReg);
            Assert.AreEqual(instr.B, bReg);

            instr = Instruction.PrintRegister(aReg);
            Assert.AreEqual(instr.Code, OpCode.PrintRegister);
            Assert.AreEqual(instr.A, aReg);
            Assert.IsNull(instr.B);
            Assert.IsNull(instr.C);

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