using SimuNet;
using Xunit;

namespace SimuNetTests
{
    public class InstructionTests
    {
        [Fact]
        public void InstructionInitTest()
        {
            Register aReg = new Register("A");
            Register bReg = new Register("B");
            Register cReg = new Register("C");

            Instruction instr = Instruction.Add(aReg, bReg, cReg);
            Assert.Equal(OpCode.Add, instr.Code);
            Assert.Equal(instr.A, aReg);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.Sub(aReg, bReg, cReg);
            Assert.Equal(OpCode.Sub, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.Mul(aReg, bReg, cReg);
            Assert.Equal(OpCode.Mul, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.Div(aReg, bReg, cReg);
            Assert.Equal(OpCode.Div, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.AddI(aReg, 5, bReg);
            Assert.Equal(OpCode.AddI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(5, instr.Immediate1);

            instr = Instruction.SubI(aReg, 5, bReg);
            Assert.Equal(OpCode.SubI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(5, instr.Immediate1);

            instr = Instruction.MulI(aReg, 5, bReg);
            Assert.Equal(OpCode.MulI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(5, instr.Immediate1);

            instr = Instruction.DivI(aReg, 5, bReg);
            Assert.Equal(OpCode.DivI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(5, instr.Immediate1);

            instr = Instruction.LoadI(aReg, 10);
            Assert.Equal(OpCode.LoadI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(10, instr.Immediate1);

            instr = Instruction.LoadMem(aReg, 7);
            Assert.Equal(OpCode.LoadMem, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(7, instr.Immediate1);

            instr = Instruction.LoadReg(aReg, bReg);
            Assert.Equal(OpCode.LoadReg, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.Move(aReg, bReg);
            Assert.Equal(OpCode.Move, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.StoreMem(aReg, 8);
            Assert.Equal(OpCode.StoreMem, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(8, instr.Immediate1);

            instr = Instruction.StoreReg(aReg, bReg);
            Assert.Equal(OpCode.StoreReg, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.Equal(aReg, bReg, cReg);
            Assert.Equal(OpCode.Equal, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.Push(aReg);
            Assert.Equal(OpCode.Push, instr.Code);
            Assert.Equal(instr.A, aReg);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.Pop(aReg);
            Assert.Equal(OpCode.Pop, instr.Code);
            Assert.Equal(instr.A, aReg);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.Jump(5);
            Assert.Equal(OpCode.Jump, instr.Code);
            Assert.Equal(5, instr.Immediate1);
            Assert.Null(instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.JumpRegister(aReg);
            Assert.Equal(OpCode.JumpRegister, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.BranchOnZero(aReg, 7);
            Assert.Equal(OpCode.BranchOnZero, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.BranchOnNotZero(aReg, 7);
            Assert.Equal(OpCode.BranchOnNotZero, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.BranchOnEqual(aReg, bReg, 7);
            Assert.Equal(OpCode.BranchOnEqual, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.BranchOnNotEqual(aReg, bReg, 7);
            Assert.Equal(OpCode.BranchOnNotEqual, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.BranchOnLessThan(aReg, bReg, 7);
            Assert.Equal(OpCode.BranchOnLessThan, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.BranchOnGreaterThan(aReg, bReg, 7);
            Assert.Equal(OpCode.BranchOnGreaterThan, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.BranchOnLessThanOrEqual(aReg, bReg, 7);
            Assert.Equal(OpCode.BranchOnLessThanOrEqual, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.BranchOnGreaterThanOrEqual(aReg, bReg, 7);
            Assert.Equal(OpCode.BranchOnGreaterThanOrEqual, instr.Code);
            Assert.Equal(7, instr.Immediate1);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.NoOp();
            Assert.Equal(OpCode.NoOp, instr.Code);
            Assert.Null(instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.Error();
            Assert.Equal(OpCode.Error, instr.Code);
            Assert.Null(instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.Exit();
            Assert.Equal(OpCode.Exit, instr.Code);
            Assert.Null(instr.A);
            Assert.Null(instr.B);
            Assert.Null(instr.C);

            instr = Instruction.LeftShift(aReg, bReg, cReg);
            Assert.Equal(OpCode.LeftShift, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.RightShift(aReg, bReg, cReg);
            Assert.Equal(OpCode.RightShift, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(bReg, instr.B);
            Assert.Equal(cReg, instr.C);

            instr = Instruction.LeftShiftI(aReg, 10, bReg);
            Assert.Equal(OpCode.LeftShiftI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(10, instr.Immediate1);
            Assert.Equal(bReg, instr.B);

            instr = Instruction.RightShiftI(aReg, 10, bReg);
            Assert.Equal(OpCode.RightShiftI, instr.Code);
            Assert.Equal(aReg, instr.A);
            Assert.Equal(10, instr.Immediate1);
            Assert.Equal(bReg, instr.B);
        }
    }
}