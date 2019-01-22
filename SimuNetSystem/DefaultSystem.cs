using SimuNet;
using SimuNetAssembler;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;

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

        /// <summary>
        ///     Assembles system provided files into the in progress program of the systems assembler.
        /// </summary>
        public void AssembleSystemFiles()
        {
            var assem = Assembly.GetAssembly(typeof(DefaultSystem));
            foreach (string macroName in assem.GetManifestResourceNames().Where(n => n.StartsWith("SimuNetSystem.Macros")))
                AssembleMacroResource(assem, macroName);
        }

        private void AssembleMacroResource(Assembly assembly, string resourceName)
        {
            var stream = assembly.GetManifestResourceStream(resourceName);
            AssembleMacro(stream);
        }

        private void AssembleMacro(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                Assembler.Assemble(reader);
            }
        }
    }
}