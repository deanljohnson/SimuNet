using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimuNet;

namespace SimuNetTests
{
    [TestClass]
    public class CPUTests
    {
        [TestMethod]
        public void CPUInitTest()
        {
            CPU cpu = new CPU();
            
            Assert.IsNotNull(cpu.V0);
            Assert.IsNotNull(cpu.V1);
            Assert.IsNotNull(cpu.V2);
            Assert.IsNotNull(cpu.V3);

            Assert.AreEqual(cpu.V0.Name, "V0");
            Assert.AreEqual(cpu.V1.Name, "V1");
            Assert.AreEqual(cpu.V2.Name, "V2");
            Assert.AreEqual(cpu.V3.Name, "V3");

            Assert.AreEqual(0, cpu.V0.Value);
            Assert.AreEqual(0, cpu.V1.Value);
            Assert.AreEqual(0, cpu.V2.Value);
            Assert.AreEqual(0, cpu.V3.Value);
        }

        [TestMethod]
        public void CPUExecuteTest()
        {
            CPU cpu = new CPU();

            Assert.ThrowsException<NullReferenceException>(() => cpu.Execute(null));
            
            cpu.Execute(Instruction.NoOp());

            Instruction instr = Instruction.Add(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 5;
            cpu.V1.Value = 7;
            cpu.Execute(instr);
            Assert.AreEqual(5, cpu.V0.Value);
            Assert.AreEqual(7, cpu.V1.Value);
            Assert.AreEqual(12, cpu.V2.Value);

            instr = Instruction.Sub(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 12;
            cpu.V1.Value = 8;
            cpu.Execute(instr);
            Assert.AreEqual(12, cpu.V0.Value);
            Assert.AreEqual(8, cpu.V1.Value);
            Assert.AreEqual(4, cpu.V2.Value);

            instr = Instruction.Mul(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 3;
            cpu.V1.Value = 9;
            cpu.Execute(instr);
            Assert.AreEqual(3, cpu.V0.Value);
            Assert.AreEqual(9, cpu.V1.Value);
            Assert.AreEqual(27, cpu.V2.Value);

            instr = Instruction.Div(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 12;
            cpu.V1.Value = 2;
            cpu.Execute(instr);
            Assert.AreEqual(12, cpu.V0.Value);
            Assert.AreEqual(2, cpu.V1.Value);
            Assert.AreEqual(6, cpu.V2.Value);

            instr = Instruction.AddI(cpu.V0, 5, cpu.V1);
            cpu.V0.Value = 5;
            cpu.Execute(instr);
            Assert.AreEqual(5, cpu.V0.Value);
            Assert.AreEqual(10, cpu.V1.Value);

            instr = Instruction.SubI(cpu.V0, 6, cpu.V1);
            cpu.V0.Value = 12;
            cpu.Execute(instr);
            Assert.AreEqual(12, cpu.V0.Value);
            Assert.AreEqual(6, cpu.V1.Value);

            instr = Instruction.MulI(cpu.V0, 7, cpu.V1);
            cpu.V0.Value = 3;
            cpu.Execute(instr);
            Assert.AreEqual(3, cpu.V0.Value);
            Assert.AreEqual(21, cpu.V1.Value);

            instr = Instruction.DivI(cpu.V0, 6, cpu.V1);
            cpu.V0.Value = 12;
            cpu.Execute(instr);
            Assert.AreEqual(12, cpu.V0.Value);
            Assert.AreEqual(2, cpu.V1.Value);

            instr = Instruction.Load(cpu.V0, 20);
            cpu.Execute(instr);
            Assert.AreEqual(20, cpu.V0.Value);

            instr = Instruction.Move(cpu.V0, cpu.V1);
            cpu.V0.Value = 10;
            cpu.V1.Value = 5;
            cpu.Execute(instr);
            Assert.AreEqual(10, cpu.V1.Value);

            instr = Instruction.Error();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cpu.Execute(instr));
        }

        [TestMethod]
        public void CPUExecuteMultipleTest()
        {
            CPU cpu = new CPU();

            Instruction load5 = Instruction.Load(cpu.V0, 5);
            Instruction load7 = Instruction.Load(cpu.V1, 7);
            Instruction add = Instruction.Add(cpu.V0, cpu.V1, cpu.V2);

            cpu.Execute(load5);
            cpu.Execute(load7);
            cpu.Execute(add);

            Assert.AreEqual(5, cpu.V0.Value);
            Assert.AreEqual(7, cpu.V1.Value);
            Assert.AreEqual(12, cpu.V2.Value);
        }

        [TestMethod]
        public void CPUProgramTest()
        {
            CPU cpu = new CPU();

            Assert.ThrowsException<InvalidOperationException>(() => cpu.RunProgram());

            Program prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 7),
                Instruction.Add(cpu.V0, cpu.V1, cpu.V2),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreEqual(12, cpu.V2.Value);

            CPU cpu2 = new CPU();
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu2.V0, 5),
                Instruction.Jump(3),
                Instruction.Load(cpu2.V1, 7),
                Instruction.Add(cpu2.V0, cpu2.V1, cpu2.V2),
                Instruction.Exit()
            });

            cpu2.LoadProgram(prog);
            cpu2.RunProgram();

            Assert.AreEqual(5, cpu2.V2.Value);
        }

        [TestMethod]
        public void CPUBranchTest()
        {
            CPU cpu = new CPU();

            // branch on zero
            Program prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 5),
                Instruction.Sub(cpu.V0, cpu.V1, cpu.V2),
                Instruction.BranchOnZero(cpu.V2, 5),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreEqual(5, cpu.V0.Value);

            cpu = new CPU();
            // Branch on not zero
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 2),
                Instruction.Sub(cpu.V0, cpu.V1, cpu.V2),
                Instruction.BranchOnNotZero(cpu.V2, 5),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreEqual(5, cpu.V0.Value);

            cpu = new CPU();
            // Branch on equal
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 5),
                Instruction.BranchOnEqual(cpu.V0, cpu.V1, 4),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreNotEqual(20, cpu.V0.Value);

            cpu = new CPU();
            // Branch on not equal
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 4),
                Instruction.BranchOnNotEqual(cpu.V0, cpu.V1, 4),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreNotEqual(20, cpu.V0.Value);

            cpu = new CPU();
            // Branch on less than
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 4),
                Instruction.Load(cpu.V1, 5),
                Instruction.BranchOnLessThan(cpu.V0, cpu.V1, 4),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreNotEqual(20, cpu.V0.Value);

            cpu = new CPU();
            // Branch on greater than
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 4),
                Instruction.BranchOnGreaterThan(cpu.V0, cpu.V1, 4),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreNotEqual(20, cpu.V0.Value);

            cpu = new CPU();
            // Branch on less than or equal
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 5),
                Instruction.BranchOnLessThanOrEqual(cpu.V0, cpu.V1, 4),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreNotEqual(20, cpu.V0.Value);

            cpu = new CPU();
            // Branch on greater than or equal
            prog = new Program(new List<Instruction>
            {
                Instruction.Load(cpu.V0, 5),
                Instruction.Load(cpu.V1, 5),
                Instruction.BranchOnGreaterThanOrEqual(cpu.V0, cpu.V1, 4),
                Instruction.Load(cpu.V0, 20),
                Instruction.Exit()
            });

            cpu.LoadProgram(prog);
            cpu.RunProgram();

            Assert.AreNotEqual(20, cpu.V0.Value);
        }
    }
}
