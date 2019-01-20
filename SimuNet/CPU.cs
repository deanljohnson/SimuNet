using System;

namespace SimuNet
{
    /// <summary>
    /// Simulates a center processing unit (CPU). A CPU loads and executes a program
    /// and provides registers in which data is stored.
    /// </summary>
    public class CPU
    {
        private readonly ALU m_ALU = new ALU();
        private readonly Stack m_Stack = new Stack(1024);
        private readonly Memory m_Memory = new Memory(65536);

        /// <summary>
        /// The program counter register. This register stores
        /// the location of the currently executing instruction.
        /// </summary>
        public Register PC { get; } = new Register("PC");

        /// <summary>
        /// First general purpose register.
        /// </summary>
        public Register V0 { get; } = new Register("V0");
        /// <summary>
        /// Second general purpose register.
        /// </summary>
        public Register V1 { get; } = new Register("V1");
        /// <summary>
        /// Third general purpose register.
        /// </summary>
        public Register V2 { get; } = new Register("V2");
        /// <summary>
        /// Fourth general purpose register.
        /// </summary>
        public Register V3 { get; } = new Register("V3");

        /// <summary>
        /// The return address register. Before performing method calls, programs must
        /// store the instruction to return to in this register. Methods are then
        /// expected to jump to this register when method execution is finished.
        /// </summary>
        public Register RA { get; } = new Register("RA");

        /// <summary>
        /// The stack pointer register.
        /// </summary>
        public Register SP { get; } = new Register("SP");

        /// <summary>
        /// The currently loaded <see cref="Program"/>
        /// or null if none is loaded.
        /// </summary>
        public Program LoadedProgram { get; private set; }

        public Action<string> Print;

        /// <summary>
        /// Loads the given program into the CPU.
        /// </summary>
        /// <param name="prog">The <see cref="Program"/> to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="prog"/> was null</exception>
        public void LoadProgram(Program prog)
        {
            LoadedProgram = prog ?? throw new ArgumentNullException(nameof(prog));
        }

        /// <summary>
        /// Runs the currently loaded program.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="LoadedProgram"/> is null.</exception>
        public void RunProgram()
        {
            if (LoadedProgram == null)
                throw new InvalidOperationException("Cannot run a program until one is loaded");

            PC.Value = 0;
            SP.Value = 0;

            while (!LoadedProgram.Finished)
            {
                Execute(LoadedProgram[PC.Value]);
                PC.Value++;
            }
        }

        internal void Execute(Instruction instr)
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
                case OpCode.LeftShift:
                case OpCode.RightShift:
                case OpCode.Equal:
                    m_ALU.DoOp(instr.Code, instr.A.Value, instr.B.Value, out result);
                    instr.C.Value = result;
                    break;
                case OpCode.AddI:
                case OpCode.SubI:
                case OpCode.MulI:
                case OpCode.DivI:
                case OpCode.LeftShiftI:
                case OpCode.RightShiftI:
                    m_ALU.DoOp(instr.Code, instr.A.Value, instr.Immediate1, out result);
                    instr.B.Value = result;
                    break;
                case OpCode.LoadI:
                    instr.A.Value = instr.Immediate1;
                    break;
                case OpCode.LoadMem:
                    instr.A.Value = m_Memory[instr.Immediate1];
                    break;
                case OpCode.LoadReg:
                    instr.A.Value = m_Memory[instr.B.Value];
                    break;
                case OpCode.Move:
                    instr.B.Value = instr.A.Value;
                    break;
                case OpCode.Push:
                    m_Stack[SP.Value + instr.Immediate1] = instr.A.Value;
                    break;
                case OpCode.Pop:
                    instr.A.Value = m_Stack[SP.Value + instr.Immediate1];
                    break;
                case OpCode.Jump:
                    PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.JumpRegister:
                    PC.Value = instr.A.Value - 1;
                    break;
                case OpCode.BranchOnZero:
                    m_ALU.DoOp(OpCode.SubI, instr.A.Value, 0, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Zero) == ALU.Flags.Zero)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnNotZero:
                    m_ALU.DoOp(OpCode.SubI, instr.A.Value, 0, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Zero) != ALU.Flags.Zero)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnEqual:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Zero) == ALU.Flags.Zero)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnNotEqual:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Zero) != ALU.Flags.Zero)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnLessThan:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Negative) == ALU.Flags.Negative)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnGreaterThan:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Positive) == ALU.Flags.Positive)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnLessThanOrEqual:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Negative) == ALU.Flags.Negative
                        || (m_ALU.StatusFlags & ALU.Flags.Zero) == ALU.Flags.Zero)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.BranchOnGreaterThanOrEqual:
                    m_ALU.DoOp(OpCode.Sub, instr.A.Value, instr.B.Value, out result);
                    if ((m_ALU.StatusFlags & ALU.Flags.Positive) == ALU.Flags.Positive
                        || (m_ALU.StatusFlags & ALU.Flags.Zero) == ALU.Flags.Zero)
                        PC.Value = instr.Immediate1 - 1;
                    break;
                case OpCode.Exit:
                    LoadedProgram.Finished = true;
                    break;
                case OpCode.PrintRegister:
                    Print?.Invoke($"{instr.A.Name}: {instr.A.Value}");
                    break;
                case OpCode.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(instr), $"The given instruction has an unrecognized opCode of {instr.Code}");
            }
        }
    }
}
