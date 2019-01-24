using System;
using System.Collections.Generic;
using SimuNet;
using Xunit;

namespace SimuNetTests
{
    public class CPUTests
    {
        [Fact]
        public void CPUInitTest()
        {
            CPU cpu = new CPU(new Memory(8));

            Assert.NotNull(cpu.V0);
            Assert.NotNull(cpu.V1);
            Assert.NotNull(cpu.V2);
            Assert.NotNull(cpu.V3);
            Assert.NotNull(cpu.RA);
            Assert.NotNull(cpu.SP);

            Assert.Equal("V0", cpu.V0.Name);
            Assert.Equal("V1", cpu.V1.Name);
            Assert.Equal("V2", cpu.V2.Name);
            Assert.Equal("V3", cpu.V3.Name);
            Assert.Equal("RA", cpu.RA.Name);
            Assert.Equal("SP", cpu.SP.Name);

            Assert.Equal(0, cpu.V0.Value);
            Assert.Equal(0, cpu.V1.Value);
            Assert.Equal(0, cpu.V2.Value);
            Assert.Equal(0, cpu.V3.Value);
            Assert.Equal(0, cpu.SP.Value);
        }

        [Fact]
        public void CPUExecuteTest()
        {
            CPU cpu = new CPU(new Memory(8));

            Assert.Throws<NullReferenceException>(() => cpu.Execute(null));

            cpu.Execute(Instruction.NoOp());

            Instruction instr = Instruction.Add(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 5;
            cpu.V1.Value = 7;
            cpu.Execute(instr);
            Assert.Equal(5, cpu.V0.Value);
            Assert.Equal(7, cpu.V1.Value);
            Assert.Equal(12, cpu.V2.Value);

            instr = Instruction.Sub(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 12;
            cpu.V1.Value = 8;
            cpu.Execute(instr);
            Assert.Equal(12, cpu.V0.Value);
            Assert.Equal(8, cpu.V1.Value);
            Assert.Equal(4, cpu.V2.Value);

            instr = Instruction.Mul(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 3;
            cpu.V1.Value = 9;
            cpu.Execute(instr);
            Assert.Equal(3, cpu.V0.Value);
            Assert.Equal(9, cpu.V1.Value);
            Assert.Equal(27, cpu.V2.Value);

            instr = Instruction.Div(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 12;
            cpu.V1.Value = 2;
            cpu.Execute(instr);
            Assert.Equal(12, cpu.V0.Value);
            Assert.Equal(2, cpu.V1.Value);
            Assert.Equal(6, cpu.V2.Value);

            instr = Instruction.AddI(cpu.V0, 5, cpu.V1);
            cpu.V0.Value = 5;
            cpu.Execute(instr);
            Assert.Equal(5, cpu.V0.Value);
            Assert.Equal(10, cpu.V1.Value);

            instr = Instruction.SubI(cpu.V0, 6, cpu.V1);
            cpu.V0.Value = 12;
            cpu.Execute(instr);
            Assert.Equal(12, cpu.V0.Value);
            Assert.Equal(6, cpu.V1.Value);

            instr = Instruction.MulI(cpu.V0, 7, cpu.V1);
            cpu.V0.Value = 3;
            cpu.Execute(instr);
            Assert.Equal(3, cpu.V0.Value);
            Assert.Equal(21, cpu.V1.Value);

            instr = Instruction.DivI(cpu.V0, 6, cpu.V1);
            cpu.V0.Value = 12;
            cpu.Execute(instr);
            Assert.Equal(12, cpu.V0.Value);
            Assert.Equal(2, cpu.V1.Value);

            instr = Instruction.LoadI(cpu.V0, 20);
            cpu.Execute(instr);
            Assert.Equal(20, cpu.V0.Value);

            instr = Instruction.Move(cpu.V0, cpu.V1);
            cpu.V0.Value = 10;
            cpu.V1.Value = 5;
            cpu.Execute(instr);
            Assert.Equal(10, cpu.V1.Value);

            instr = Instruction.JumpRegister(cpu.V0);
            cpu.V0.Value = 10;
            cpu.Execute(instr);
            // 1 less than the target register because the CPU will
            // increment this before moving to the next instruction.
            Assert.Equal(9, cpu.PC.Value);

            instr = Instruction.Error();
            Assert.Throws<ArgumentOutOfRangeException>(() => cpu.Execute(instr));
        }

        [Fact]
        public void CPUExecuteMultipleTest()
        {
            CPU cpu = new CPU(new Memory(8));

            Instruction load5 = Instruction.LoadI(cpu.V0, 5);
            Instruction load7 = Instruction.LoadI(cpu.V1, 7);
            Instruction add = Instruction.Add(cpu.V0, cpu.V1, cpu.V2);

            cpu.Execute(load5);
            cpu.Execute(load7);
            cpu.Execute(add);

            Assert.Equal(5, cpu.V0.Value);
            Assert.Equal(7, cpu.V1.Value);
            Assert.Equal(12, cpu.V2.Value);
        }

        [Fact]
        public void CPUProgramTest()
        {
            CPU cpu = new CPU(new Memory(8));

            Assert.Throws<InvalidOperationException>(() => cpu.RunProgram());

            Program prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 7),
                Instruction.Add(cpu.V0, cpu.V1, cpu.V2),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.Equal(12, cpu.V2.Value);

            CPU cpu2 = new CPU(new Memory(8));
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu2.V0, 5),
                Instruction.Jump(3),
                Instruction.LoadI(cpu2.V1, 7),
                Instruction.Add(cpu2.V0, cpu2.V1, cpu2.V2),
                Instruction.Exit()
            });

            cpu2.LoadProgram(prog);
            cpu2.RunProgram();

            Assert.Equal(5, cpu2.V2.Value);
        }

        [Fact]
        public void CPULoadNullProgramTest()
        {
            CPU cpu = new CPU(new Memory(8));
            Assert.Throws<ArgumentNullException>(() => cpu.LoadProgram(null));
        }

        [Fact]
        public void CPUBranchTest()
        {
            CPU cpu = new CPU(new Memory(8));

            // branch on zero
            Program prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.Sub(cpu.V0, cpu.V1, cpu.V2),
                Instruction.BranchOnZero(cpu.V2, 5),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on not zero
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 2),
                Instruction.Sub(cpu.V0, cpu.V1, cpu.V2),
                Instruction.BranchOnNotZero(cpu.V2, 5),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on equal
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.BranchOnEqual(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on not equal
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 4),
                Instruction.BranchOnNotEqual(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on less than
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 4),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.BranchOnLessThan(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on greater than
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 4),
                Instruction.BranchOnGreaterThan(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on less than or equal, equal case
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.BranchOnLessThanOrEqual(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on less than or equal, less than case
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 4),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.BranchOnLessThanOrEqual(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on greater than or equal, equal case
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 5),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.BranchOnGreaterThanOrEqual(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);

            cpu = new CPU(new Memory(8));
            // Branch on greater than or equal, greater than case
            prog = new Program(new List<Instruction>
            {
                Instruction.LoadI(cpu.V0, 6),
                Instruction.LoadI(cpu.V1, 5),
                Instruction.BranchOnGreaterThanOrEqual(cpu.V0, cpu.V1, 4),
                Instruction.LoadI(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.NotEqual(20, cpu.V0.Value);
        }

        [Fact]
        public void CPUStack()
        {
            const int STACK_SIZE = 1024;
            CPU cpu = new CPU(new Memory(8));

            for (int i = 0; i < STACK_SIZE; i++)
            {
                cpu.Execute(Instruction.LoadI(cpu.V0, i));
                cpu.Execute(Instruction.Push(cpu.V0));
            }

            for (int i = 0; i < STACK_SIZE; i++)
            {
                cpu.Execute(Instruction.Pop(cpu.V1));
                Assert.Equal(STACK_SIZE - i - 1, cpu.V1.Value);
            }
        }

        [Fact]
        public void CPUShiftOperations()
        {
            CPU cpu = new CPU(new Memory(8));
            cpu.V0.Value = 1;
            cpu.V1.Value = 1;
            cpu.Execute(Instruction.LeftShift(cpu.V0, cpu.V1, cpu.V0));
            Assert.Equal(2, cpu.V0.Value);
            cpu.Execute(Instruction.RightShift(cpu.V0, cpu.V1, cpu.V0));
            Assert.Equal(1, cpu.V0.Value);
            cpu.Execute(Instruction.LeftShiftI(cpu.V0, 1, cpu.V0));
            Assert.Equal(2, cpu.V0.Value);
            cpu.Execute(Instruction.RightShiftI(cpu.V0, 1, cpu.V0));
            Assert.Equal(1, cpu.V0.Value);
        }

        [Fact]
        public void CPUMemOperations()
        {
            Memory memory = new Memory(1);
            CPU cpu = new CPU(memory);

            cpu.V0.Value = 5;
            cpu.Execute(Instruction.StoreMem(cpu.V0, 0));
            Assert.Equal(5, memory[0]);

            memory[0] = 10;
            cpu.Execute(Instruction.LoadMem(cpu.V0, 0));
            Assert.Equal(10, cpu.V0.Value);

            cpu.V0.Value = 15;
            cpu.V1.Value = 0;
            cpu.Execute(Instruction.StoreReg(cpu.V0, cpu.V1));
            Assert.Equal(15, memory[0]);

            memory[0] = 20;
            cpu.V1.Value = 0;
            cpu.Execute(Instruction.LoadReg(cpu.V0, cpu.V1));
            Assert.Equal(20, cpu.V0.Value);

        }
    }
}
