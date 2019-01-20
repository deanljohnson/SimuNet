using System;
using System.Collections.Generic;
using System.IO;
using SimuNet;
using System.Linq;

namespace SimuNetAssembler
{
    public class Assembler
    {
        private readonly CPU m_CPU;
        private readonly MacroAssembler m_MacroAssembler;
        private readonly Dictionary<string, int> m_Labels = new Dictionary<string, int>();
        private delegate bool LabelFoundDelegate(string labelText);
        private readonly List<LabelFoundDelegate> m_PendingLabelReferences = new List<LabelFoundDelegate>();

        public Assembler(CPU cpu)
        {
            m_CPU = cpu;
            m_MacroAssembler = new MacroAssembler();
        }

        public Program Assemble(FileInfo file)
        {
            using (StreamReader reader = file.OpenText())
                return Assemble(reader);
        }

        public Program Assemble(StreamReader reader)
        {
            List<ParsedInstruction> instructions = new List<ParsedInstruction>();

            int lineNumber = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().ToLowerInvariant().Trim();
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (IsComment(line))
                    continue;

                if (m_MacroAssembler.IsBeginMacro(line))
                {
                    m_MacroAssembler.BeginMacro(line, lineNumber);
                }
                else if (m_MacroAssembler.IsEndMacro(line))
                {
                    m_MacroAssembler.EndMacro(lineNumber);
                }
                else if (m_MacroAssembler.InMacro)
                {
                    m_MacroAssembler.AddToMacroSource(line);
                }
                else
                {
                    try
                    {
                        ParseLine(line, lineNumber, instructions);
                    }
                    catch (ArgumentException e)
                    {
                        throw new InvalidOperationException($"Parsing line {lineNumber} failed: {e.Message}");
                    }
                }
            }

            foreach (var pInstruction in instructions)
            {
                if (pInstruction.Instruction == null)
                    throw new InvalidOperationException($"Line {pInstruction.LineNumber} was not successfully resolved. This is likely due to a missing label definition.");
            }

            return new Program(instructions.Select(p => p.Instruction));
        }

        private void ParseLine(string line, int lineNumber, List<ParsedInstruction> instructions)
        {
            ParsedInstruction instr = new ParsedInstruction
            {
                Line = line,
                LineNumber = lineNumber,
                InstructionNumber = instructions.Count,
                Tokens = line.Split(' ')
            };

            string label = instr.Tokens[0].EndsWith(":") ? ParseLabel(instr.Tokens[0]) : null;
            if (label != null)
            {
                instr.Label = label;
                ProcessLabel(instr.InstructionNumber, label);
            }

            // If this line has a label but no instruction,
            // such as "label:", insert a No-Op instruction on the line.
            // Keeps label jumps simple.
            if (instr.Label != null && instr.Tokens.Length == 1)
            {
                instr.Instruction = Instruction.NoOp();
                instructions.Add(instr);
                return;
            }

            instr.OpIndex = instr.Label == null ? 0 : 1;
            string opToken = instr.Tokens[instr.OpIndex];
            if (m_MacroAssembler.TryGetMacro(opToken, out Macro macro))
            {
                string[] macroSource = macro.SubstituteArguments(instr.Tokens.AsSpan().Slice(instr.OpIndex + 1));

                for (int i = 0; i < macroSource.Length; i++)
                {
                    ParseLine(macroSource[i], lineNumber, instructions);
                }
            }
            else
            {
                instr.OpCode = ParseOpCode(opToken);

                if (IsBranching(instr.OpCode))
                    ParseBranchingInstruction(instr);
                else
                    instr.Instruction = ParseSimpleInstruction(instr);

                instructions.Add(instr);
            }
        }

        private void ProcessLabel(int instrNumber, string label)
        {
            if (!m_Labels.ContainsKey(label))
            {
                m_Labels.Add(label, instrNumber);

                for (int i = 0; i < m_PendingLabelReferences.Count; i++)
                {
                    bool resolved = m_PendingLabelReferences[i].Invoke(label);
                    if (resolved)
                    {
                        m_PendingLabelReferences.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private Instruction ParseSimpleInstruction(ParsedInstruction instr)
        {
            int opIndex = instr.OpIndex;
            string[] tokens = instr.Tokens;
            switch (instr.OpCode)
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
                case OpCode.JumpRegister:
                    return Instruction.JumpRegister(ParseRegister(tokens[opIndex + 1]));
                case OpCode.PrintRegister:
                    return Instruction.PrintRegister(ParseRegister(tokens[opIndex + 1]));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ParseBranchingInstruction(ParsedInstruction instr)
        {
            int opIndex = instr.OpIndex;
            string[] tokens = instr.Tokens;

            bool unresolved = false;
            string unresolvedToken = null;
            for (int i = tokens.Length - 1; i > opIndex; i--)
            {
                if (IsUnresolvedLabel(tokens[i]))
                {
                    unresolved = true;
                    unresolvedToken = tokens[i];
                    break;
                }
            }

            if (unresolved)
            {
                bool LabelFound(string label)
                {
                    if (unresolvedToken != label)
                        return false;

                    ParseBranchingInstruction(instr);

                    return true;
                }

                m_PendingLabelReferences.Add(LabelFound);
                return;
            }

            switch (instr.OpCode)
            {
                case OpCode.Jump:
                    instr.Instruction = Instruction.Jump(ParseInstructionNumber(tokens[opIndex + 1]));
                    break;
                case OpCode.BranchOnZero:
                    instr.Instruction = Instruction.BranchOnZero(ParseRegister(tokens[opIndex + 1]), ParseInstructionNumber(tokens[opIndex + 2]));
                    break;
                case OpCode.BranchOnNotZero:
                    instr.Instruction = Instruction.BranchOnNotZero(ParseRegister(tokens[opIndex + 1]), ParseInstructionNumber(tokens[opIndex + 2]));
                    break;
                case OpCode.BranchOnEqual:
                    instr.Instruction = Instruction.BranchOnEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                    break;
                case OpCode.BranchOnNotEqual:
                    instr.Instruction = Instruction.BranchOnNotEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                    break;
                case OpCode.BranchOnLessThan:
                    instr.Instruction = Instruction.BranchOnLessThan(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                    break;
                case OpCode.BranchOnGreaterThan:
                    instr.Instruction = Instruction.BranchOnGreaterThan(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                    break;
                case OpCode.BranchOnLessThanOrEqual:
                    instr.Instruction = Instruction.BranchOnLessThanOrEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                    break;
                case OpCode.BranchOnGreaterThanOrEqual:
                    instr.Instruction = Instruction.BranchOnGreaterThanOrEqual(ParseRegister(tokens[opIndex + 1]), ParseRegister(tokens[opIndex + 2]), ParseInstructionNumber(tokens[opIndex + 3]));
                    break;
                case OpCode.PrintRegister:
                    instr.Instruction = Instruction.PrintRegister(ParseRegister(tokens[opIndex + 1]));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsBranching(OpCode code)
        {
            switch (code)
            {
                case OpCode.BranchOnEqual:
                case OpCode.BranchOnGreaterThan:
                case OpCode.BranchOnGreaterThanOrEqual:
                case OpCode.BranchOnLessThan:
                case OpCode.BranchOnLessThanOrEqual:
                case OpCode.BranchOnNotEqual:
                case OpCode.BranchOnNotZero:
                case OpCode.BranchOnZero:
                case OpCode.Jump:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsComment(string line)
        {
            return line.Trim().StartsWith("//");
        }

        private bool IsUnresolvedLabel(string token)
        {
            if (int.TryParse(token, out _))
                return false;
            if (m_Labels.ContainsKey(token))
                return false;

            try
            {
                ParseRegister(token);
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
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
            return -1;
        }
    }
}
