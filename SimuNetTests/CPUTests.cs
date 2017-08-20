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

            Assert.AreEqual(cpu.V0.Value, 0);
            Assert.AreEqual(cpu.V1.Value, 0);
            Assert.AreEqual(cpu.V2.Value, 0);
            Assert.AreEqual(cpu.V3.Value, 0);
        }

        [TestMethod]
        public void CPUExecuteTest()
        {
            CPU cpu = new CPU();

            Assert.ThrowsException<NullReferenceException>(() => cpu.Execute(null));

            Instruction instr = Instruction.Add(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 5;
            cpu.V1.Value = 7;
            cpu.Execute(instr);
            Assert.AreEqual(cpu.V0.Value, 5);
            Assert.AreEqual(cpu.V1.Value, 7);
            Assert.AreEqual(cpu.V2.Value, 12);

            instr = Instruction.Sub(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 12;
            cpu.V1.Value = 8;
            cpu.Execute(instr);
            Assert.AreEqual(cpu.V0.Value, 12);
            Assert.AreEqual(cpu.V1.Value, 8);
            Assert.AreEqual(cpu.V2.Value, 4);

            instr = Instruction.Mul(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 3;
            cpu.V1.Value = 9;
            cpu.Execute(instr);
            Assert.AreEqual(cpu.V0.Value, 3);
            Assert.AreEqual(cpu.V1.Value, 9);
            Assert.AreEqual(cpu.V2.Value, 27);

            instr = Instruction.Div(cpu.V0, cpu.V1, cpu.V2);
            cpu.V0.Value = 12;
            cpu.V1.Value = 2;
            cpu.Execute(instr);
            Assert.AreEqual(cpu.V0.Value, 12);
            Assert.AreEqual(cpu.V1.Value, 2);
            Assert.AreEqual(cpu.V2.Value, 6);

            instr = Instruction.Load(cpu.V0, 20);
            cpu.Execute(instr);
            Assert.AreEqual(cpu.V0.Value, 20);

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

            Assert.AreEqual(cpu.V0.Value, 5);
            Assert.AreEqual(cpu.V1.Value, 7);
            Assert.AreEqual(cpu.V2.Value, 12);
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

            Assert.AreEqual(cpu.V2.Value, 12);

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

            Assert.AreEqual(cpu2.V2.Value, 5);
        }
    }
}
