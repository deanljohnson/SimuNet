using SimuNet;

namespace SimuNetAssembler
{
    public class ParsedInstruction
    {
        public string Line { get; set; }
        public int LineNumber { get; set; }
        public int InstructionNumber { get; set; }
        public string Label { get; set; }
        public string[] Tokens { get; set; }
        public OpCode OpCode { get; set; }
        public int OpIndex { get; set; }
        public Instruction Instruction { get; set; }
    }
}
