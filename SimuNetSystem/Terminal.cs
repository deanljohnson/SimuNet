using SimuNet;
using System;

namespace SimuNetSystem
{
    /// <summary>
    ///     Provides a system device that can display textual data on a console display. 
    /// </summary>
    public class Terminal
    {
        private readonly MemoryMap m_MappedIO;
        public Terminal(int baseAddress)
        {
            m_MappedIO = new MemoryMap(baseAddress, 16);
            m_MappedIO.MemorySet += OnMemorySet;
            m_MappedIO.MemoryGet += OnMemoryGet;
        }

        public void LoadIntoMemory(Memory mem)
        {
            mem.AddMemoryMap(m_MappedIO);
        }

        private void OnMemorySet(int addr, int value)
        {
            // Base address indicates a single ASCII character for printing
            if (addr == m_MappedIO.BaseAddress)
            {
                char character = (char)value;
                Console.Write(character);
            }
        }

        private int OnMemoryGet(int addr)
        {
            // TODO: should Terminal store some buffer of data?
            return 0;
        }
    }
}