using SimuNet;
using SimuNetAssembler;
using System.IO;
using System.Text;

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

            LoadDefaultMacros();
        }

        private void LoadDefaultMacros()
        {
            AssembleMacroSource("#begin print $1\nstorem $1 65536\n#end");
        }

        private void AssembleMacroSource(string source)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(source);
            using (var stream = new MemoryStream(bytes))
            using (var reader = new StreamReader(stream))
            {
                Assembler.Assemble(reader);
            }
        }
    }
}