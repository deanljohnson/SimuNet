using System;

namespace SimuNet
{
    public class CPU
    {
        private readonly ALU m_ALU = new ALU();

        public Register PC { get; } = new Register("PC");

        public Register V0 { get; } = new Register("V0");
        public Register V1 { get; } = new Register("V1");
        public Register V2 { get; } = new Register("V2");
        public Register V3 { get; } = new Register("V3");

        public Program LoadedProgram { get; private set; }

        public void LoadProgram(Program prog)
        {
            LoadedProgram = prog;
        }

        public void RunProgram()
        {
            if (LoadedProgram == null)
                throw new InvalidOperationException("Cannot run a program until one is loaded");

            PC.Value = 0;

            while (!LoadedProgram.Finished)
            {
                Execute(LoadedProgram[PC.Value]);
                PC.Value++;
            }
        }

        public void Execute(Instruction instr)
        {
            int result;
            switch (instr.Code)
            {
                case OpCode.NoOp:
                    break;
                case OpCode.Add:
                case OpCode.Sub:
                case OpCode.Mul:
                case OpCode.Div:
                case OpCode.Equal:
                    m_ALU.DoOp(instr.Code, instr.A.Value, instr.B.Value, out result);
                    instr.C.Value = result;
                    break;
                case OpCode.Load:
                    instr.A.Value = instr.Immediate1;
                    break;
                case OpCode.Jump:
                    PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnZero:
                    m_ALU.DoOp(OpCode.Equal, instr.A.Value, 0, out result);
                    if (result == 1)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnNotZero:
                    m_ALU.DoOp(OpCode.Equal, instr.A.Value, 0, out result);
                    if (result == 0)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnEqual:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if (result == 0)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.Exit:
                    LoadedProgram.Finished = true;
                    break;
                case OpCode.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(instr), $"The given instruction has an unrecognized opCode of {instr.Code}");
            }
        }
    }
}
