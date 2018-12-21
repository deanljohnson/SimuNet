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
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().ToLowerInvariant();
                    if (IsComment(line))
                        continue;

                    string[] tokens = line.Split(' ');

                    string label = tokens[0].EndsWith(":") ? ParseLabel(tokens[0]) : null;
                    int opIndex = label == null ? 0 : 1;
                    OpCode op = ParseOpCode(tokens[opIndex]);

                    switch (op)
                    {
                        case OpCode.Error:
                            instructions.Add(Instruction.Error());
                            break;
                        case OpCode.NoOp:
                            instructions.Add(Instruction.NoOp());
                            break;
                        case OpCode.Exit:
                            instructions.Add(Instruction.Exit());
                            break;
                        case OpCode.Add:
                            instructions.Add(Instruction.Add(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.Sub:
                            instructions.Add(Instruction.Sub(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.Mul:
                            instructions.Add(Instruction.Mul(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.Div:
                            instructions.Add(Instruction.Div(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.AddI:
                            instructions.Add(Instruction.AddI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.SubI:
                            instructions.Add(Instruction.SubI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.MulI:
                            instructions.Add(Instruction.MulI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.DivI:
                            instructions.Add(Instruction.DivI(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.Equal:
                            instructions.Add(Instruction.Equal(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseRegister(tokens[opIndex + 3])));
                            break;
                        case OpCode.Load:
                            instructions.Add(Instruction.Load(ParseRegister(tokens[opIndex + 1]), ParseImmediate(tokens[opIndex + 2])));
                            break;
                        case OpCode.Move:
                            instructions.Add(Instruction.Move(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2])));
                            break;
                        case OpCode.Jump:
                            instructions.Add(Instruction.Jump(ParseInstructionNumber(tokens[opIndex + 1])));
                            break;
                        case OpCode.BranchOnZero:
                            instructions.Add(Instruction.BranchOnZero(ParseRegister(tokens[opIndex + 1]), ParseInstructionNumber(tokens[opIndex + 2])));
                            break;
                        case OpCode.BranchOnNotZero:
                            instructions.Add(Instruction.BranchOnNotZero(ParseRegister(tokens[opIndex + 1]), ParseInstructionNumber(tokens[opIndex + 2])));
                            break;
                        case OpCode.BranchOnEqual:
                            instructions.Add(Instruction.BranchOnEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3])));
                            break;
                        case OpCode.BranchOnNotEqual:
                            instructions.Add(Instruction.BranchOnNotEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3])));
                            break;
                        case OpCode.BranchOnLessThan:
                            instructions.Add(Instruction.BranchOnLessThan(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3])));
                            break;
                        case OpCode.BranchOnGreaterThan:
                            instructions.Add(Instruction.BranchOnGreaterThan(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3])));
                            break;
                        case OpCode.BranchOnLessThanOrEqual:
                            instructions.Add(Instruction.BranchOnLessThanOrEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3])));
                            break;
                        case OpCode.BranchOnGreaterThanOrEqual:
                            instructions.Add(Instruction.BranchOnGreaterThanOrEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3])));
                            break;
                        case OpCode.PrintRegister:
                            instructions.Add(Instruction.PrintRegister(ParseRegister(tokens[opIndex + 1])));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return new Program(instructions);
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

                    if (IsComment(line))
                        continue;

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
                case "addi": return OpCode.AddI;
                case "subi": return OpCode.SubI;
                case "muli": return OpCode.MulI;
                case "divi": return OpCode.DivI;
                case "equal": return OpCode.Equal;
                case "load": return OpCode.Load;
                case "move": return OpCode.Move;
                case "jump": return OpCode.Jump;
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
            throw new ArgumentException($"Unrecognized register {token}", nameof(token));
        }

        private static int ParseImmediate(string token)
        {
            if (int.TryParse(token, out int i))
                return i;
            throw new ArgumentException($"Invalid immediate value: {token}", nameof(token));
        }

        private int ParseInstructionNumber(string token)
        {
            if (int.TryParse(token, out int i))
                return i;
            if (m_Labels.TryGetValue(token, out i))
                return i;

            throw new KeyNotFoundException($"Could not find a line number for the given label {token}");
        }
    }
}
