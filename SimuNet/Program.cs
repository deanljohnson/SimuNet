using System.Collections.Generic;
using System.Linq;

namespace SimuNet
{
    public class Program
    {
        private readonly List<Instruction> m_Instructions;

        public int InstructionCount => m_Instructions.Count;

        public Instruction this[int i] => m_Instructions[i];

        public bool Finished { get; set; }

        public Program(IEnumerable<Instruction> instructions)
        {
            m_Instructions = instructions.ToList();
        }
    }
}
