using SimuNet;
using SimuNetAssembler;

namespace SimuNetSystem
{
    public class DefaultSystem
    {
        public Memory Memory { get; }
        public CPU CPU { get; }
        public Assembler Assembler { get; }

        public DefaultSystem()
        {
            Memory = new Memory(65536);

            var terminal = new Terminal(65536);
            terminal.LoadIntoMemory(Memory);

            CPU = new CPU(Memory);

            Assembler = new Assembler(CPU);
        }
    }
}