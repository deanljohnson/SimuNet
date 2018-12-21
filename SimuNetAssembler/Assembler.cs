using System;
using System.Collections.Generic;
using System.IO;
using SimuNet;

namespace SimuNetAssembler
{
    public class Assembler
    {
        private readonly CPU m_CPU;
        private readonly Dictionary<string, int> m_Labels = new Dictionary<string, int>();

        public Assembler(CPU cpu)
        {
            m_CPU = cpu;
        }

        public Program Assemble(FileInfo file)
        {
            List<Instruction> instructions = new List<Instruction>();

            ReadLabels(file);

            using (StreamReader reader = file.OpenText())
            {
                int lineNumber = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().ToLowerInvariant();
                    lineNumber++;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    if (IsComment(line))
                        continue;

                    string[] tokens = line.Split(' ');

                    try
                    {
                        instructions.Add(ParseInstruction(tokens));
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidOperationException($"Parsing line {lineNumber} failed: {e.Message}");
                    }
                }
            }

            return new Program(instructions);
        }

        private Instruction ParseInstruction(string[] tokens)
        {
            string label = tokens[0].EndsWith(":") ? ParseLabel(tokens[0]) : null;

            // If this line has a label but no instruction,
            // such as "label:", insert a No-Op instruction on the line.
            // Keeps label jumps simple.
            if (label != null && tokens.Length == 1)
            {
                return Instruction.NoOp();
            }

            int opIndex = label == null ? 0 : 1;
            OpCode op = ParseOpCode(tokens[opIndex]);

            switch (op)
            {
                case OpCode.Error:
                    return Instruction.Error();
                case OpCode.NoOp:
                    return Instruction.NoOp();
                case OpCode.Exit:
                    return Instruction.Exit();
                case OpCode.Add:
                    return Instruction.Add(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.Sub:
                    return Instruction.Sub(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.Mul:
                    return Instruction.Mul(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.Div:
                    return Instruction.Div(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.LeftShift:
                    return Instruction.LeftShift(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.RightShift:
                    return Instruction.RightShift(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.AddI:
                    return Instruction.AddI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.SubI:
                    return Instruction.SubI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.MulI:
                    return Instruction.MulI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.DivI:
                    return Instruction.DivI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.LeftShiftI:
                    return Instruction.LeftShiftI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.RightShiftI:
                    return Instruction.RightShiftI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.Equal:
                    return Instruction.Equal(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3]));
                case OpCode.Load:
                    return Instruction.Load(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]));
                case OpCode.Move:
                    return Instruction.Move(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]));
                case OpCode.Push:
                    return Instruction.Push(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]));
                case OpCode.Pop:
                    return Instruction.Pop(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]));
                case OpCode.Jump:
                    return Instruction.Jump(ParseInstructionNumber(tokens[opIndex + 1]));
                case OpCode.JumpRegister:
                    return Instruction.JumpRegister(ParseRegister(tokens[opIndex + 1]));
                case OpCode.BranchOnZero:
                    return Instruction.BranchOnZero(ParseRegister(tokens[opIndex + 1]), ParseInstructionNumber(tokens[opIndex + 2]));
                case OpCode.BranchOnNotZero:
                    return Instruction.BranchOnNotZero(ParseRegister(tokens[opIndex + 1]), ParseInstructionNumber(tokens[opIndex + 2]));
                case OpCode.BranchOnEqual:
                    return Instruction.BranchOnEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                case OpCode.BranchOnNotEqual:
                    return Instruction.BranchOnNotEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                case OpCode.BranchOnLessThan:
                    return Instruction.BranchOnLessThan(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                case OpCode.BranchOnGreaterThan:
                    return Instruction.BranchOnGreaterThan(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                case OpCode.BranchOnLessThanOrEqual:
                    return Instruction.BranchOnLessThanOrEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                case OpCode.BranchOnGreaterThanOrEqual:
                    return Instruction.BranchOnGreaterThanOrEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                case OpCode.PrintRegister:
                    return Instruction.PrintRegister(ParseRegister(tokens[opIndex + 1]));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ReadLabels(FileInfo file)
        {
            m_Labels.Clear();

            using (StreamReader labelReader = file.OpenText())
            {
                int currentLine = -1;

                while (!labelReader.EndOfStream)
                {
                    currentLine++;
                    string line = labelReader.ReadLine().ToLowerInvariant();

                    if (string.IsNullOrWhiteSpace(line)
                        || IsComment(line))
                    {
                        currentLine--;
                        continue;
                    }

                    string[] tokens = line.Split(' ');

                    if (!tokens[0].EndsWith(":"))
                        continue;

                    m_Labels[ParseLabel(tokens[0])] = currentLine;
                }
            }
        }

        private bool IsComment(string line)
        {
            return line.Trim().StartsWith("//");
        }

        private static string ParseLabel(string token)
        {
            string label = token.Remove(token.Length - 1, 1);
            if (label.Length == 0)
                throw new ArgumentException($"{token} is not a valid label identifier", nameof(token));

            return label;
        }

        private static OpCode ParseOpCode(string token)
        {
            switch (token)
            {
                case "noop": return OpCode.NoOp;
                case "exit": return OpCode.Exit;
                case "add": return OpCode.Add;
                case "sub": return OpCode.Sub;
                case "mul": return OpCode.Mul;
                case "div": return OpCode.Div;
                case "ls": return OpCode.LeftShift;
                case "rs": return OpCode.RightShift;
                case "addi": return OpCode.AddI;
                case "subi": return OpCode.SubI;
                case "muli": return OpCode.MulI;
                case "divi": return OpCode.DivI;
                case "lsi": return OpCode.LeftShiftI;
                case "rsi": return OpCode.RightShiftI;
                case "equal": return OpCode.Equal;
                case "load": return OpCode.Load;
                case "move": return OpCode.Move;
                case "push": return OpCode.Push;
                case "pop": return OpCode.Pop;
                case "jump": return OpCode.Jump;
                case "jumpr": return OpCode.JumpRegister;
                case "boz": return OpCode.BranchOnZero;
                case "bonz": return OpCode.BranchOnNotZero;
                case "boe": return OpCode.BranchOnEqual;
                case "bone": return OpCode.BranchOnNotEqual;
                case "bolt": return OpCode.BranchOnLessThan;
                case "bogt": return OpCode.BranchOnGreaterThan;
                case "bolte": return OpCode.BranchOnLessThanOrEqual;
                case "bogte": return OpCode.BranchOnGreaterThanOrEqual;
                case "print": return OpCode.PrintRegister;
                default: throw new ArgumentException($"{token} is not a recognized instruction", nameof(token));
            }
        }

        private Register ParseRegister(string token)
        {
            if (m_CPU.V0.Name.ToLowerInvariant() == token) return m_CPU.V0;
            if (m_CPU.V1.Name.ToLowerInvariant() == token) return m_CPU.V1;
            if (m_CPU.V2.Name.ToLowerInvariant() == token) return m_CPU.V2;
            if (m_CPU.V3.Name.ToLowerInvariant() == token) return m_CPU.V3;
            if (m_CPU.PC.Name.ToLowerInvariant() == token) return m_CPU.PC;
            if (m_CPU.RA.Name.ToLowerInvariant() == token) return m_CPU.RA;
            if (m_CPU.SP.Name.ToLowerInvariant() == token) return m_CPU.SP;
            throw new ArgumentException($"Unrecognized register {token}", nameof(token));
        }

        private static int ParseImmediate(string token)
        {
            if (int.TryParse(token, out int i))
                return i;
            throw new ArgumentException($"Invalid immediate value {token}", nameof(token));
        }

        private int ParseInstructionNumber(string token)
        {
            if (int.TryParse(token, out int i))
                return i;
            if (m_Labels.TryGetValue(token, out i))
                return i;

            throw new KeyNotFoundException($"Could not find a line number for the label {token}");
        }
    }
}
