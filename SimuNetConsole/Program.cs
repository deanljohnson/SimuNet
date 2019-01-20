using System;
using System.IO;
using SimuNet;
using SimuNetAssembler;

namespace SimuNetConsole
{
    class Program
    {
        private static void Main(string[] args)
        {
            string fileName = "Programs/fib-recursive.txt";
            if (args.Length == 1)
                fileName = args[0];

            CPU cpu = new CPU();
            Assembler assem = new Assembler(cpu);
            cpu.Print = Console.WriteLine;

            cpu.LoadProgram(assem.Assemble(new FileInfo(fileName)));
            cpu.RunProgram();

            Console.ReadLine();
        }
    }
}
