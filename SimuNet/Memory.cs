using System;

namespace SimuNet
{
    /// <summary>
    ///     Simulates a systems RAM space with support for memory mapped regions.
    /// </summary>
    public class Memory
    {
        private readonly int[] m_Memory;

        public int this[int address]
        {
            get
            {
                if (address < 0)
                    throw new InvalidOperationException("Out of Bounds Memory Access");
                if (address < m_Memory.Length)
                    return m_Memory[address];
                else
                {
                    // TODO: check memory maps
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Creates a memory instance of the given size.
        /// </summary>
        /// <param name="mainMemorySize">The size of the RAM space in 32 bit words.</param>
        public Memory(int mainMemorySize)
        {
            m_Memory = new int[mainMemorySize];
        }
    }
}