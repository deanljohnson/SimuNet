using System;
using System.IO;
using SimuNet;
using SimuNetAssembler;
using SimuNetSystem;

namespace SimuNetConsole
{
    class Program
    {
        private static void Main(string[] args)
        {
            string fileName = "Programs/fib-recursive.txt";
            if (args.Length == 1)
                fileName = args[0];

            DefaultSystem system = new DefaultSystem();

            system.CPU.LoadProgram(system.Assembler.Assemble(new FileInfo(fileName)));
            system.CPU.RunProgram();

            Console.ReadLine();
        }
    }
}
