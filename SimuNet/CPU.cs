using System;

namespace SimuNet
{
    public class CPU
    {
        private readonly ALU m_ALU = new ALU();
        private int m_ProgramCounter = 0;

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

            m_ProgramCounter = 0;

            while (!LoadedProgram.Finished)
            {
                Instruction instr = LoadedProgram[m_ProgramCounter];
                switch (instr.Code)
                {
                    case OpCode.Exit:
                        LoadedProgram.Finished = true;
                        break;
                    case OpCode.Jump:
                        m_ProgramCounter = instr.Immediate1;
                        break;
                    default:
                        Execute(instr);
                        m_ProgramCounter++;
                        break;
                }
            }
        }

        public void Execute(Instruction instr)
        {
            switch (instr.Code)
            {
                case OpCode.NoOp:
                    break;
                case OpCode.Add:
                case OpCode.Sub:
                case OpCode.Mul:
                case OpCode.Div:
                    m_ALU.DoOp(instr.Code, instr.A.Value, instr.B.Value, out int result);
                    instr.C.Value = result;
                    break;
                case OpCode.Load:
                    instr.A.Value = instr.Immediate1;
                    break;
                case OpCode.Exit:
                    break;
                case OpCode.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(instr), $"The given instruction has an unrecognized opCode of {instr.Code}");
            }
        }
    }
}
