using System;
using System.Collections.Generic;

namespace SimuNet
{
    /// <summary>
    ///     Simulates a systems RAM space with support for memory mapped regions.
    /// </summary>
    public class Memory
    {
        private readonly int[] m_Memory;
        private readonly List<MemoryMap> m_MemoryMaps = new List<MemoryMap>();

        public int this[int address]
        {
            get
            {
                if (address < 0)
                    throw new InvalidOperationException("Out of Bounds Memory Access");
                if (address < m_Memory.Length)
                    return m_Memory[address];

                foreach (var map in m_MemoryMaps)
                {
                    if (address >= map.BaseAddress && address < map.BaseAddress + map.Length)
                    {
                        return map.Get(address);
                    }
                }

                throw new InvalidOperationException("Out of Bounds Memory Access");
            }
            set
            {
                if (address < 0)
                    throw new InvalidOperationException("Out of Bounds Memory Access");
                if (address < m_Memory.Length)
                    m_Memory[address] = value;
                else
                {
                    foreach (var map in m_MemoryMaps)
                    {
                        if (address >= map.BaseAddress && address < map.BaseAddress + map.Length)
                        {
                            map.Set(address, value);
                            return;
                        }
                    }
                    throw new InvalidOperationException("Out of Bounds Memory Access");
                }
            }
        }

        /// <summary>
        ///     Creates a memory instance of the given size.
        /// </summary>
        /// <param name="mainMemorySize">The size of the RAM space in 32 bit words.</param>
        public Memory(int mainMemorySize)
        {
            if (mainMemorySize < 0)
                throw new ArgumentOutOfRangeException(nameof(mainMemorySize));
            m_Memory = new int[mainMemorySize];
        }

        public void AddMemoryMap(MemoryMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (map.BaseAddress < m_Memory.Length)
                throw new InvalidOperationException("Memory maps cannot overlap main memory");
            // TODO: check for overlap with all existing maps
            m_MemoryMaps.Add(map);
        }

        public void RemoveMemoryMap(MemoryMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            m_MemoryMaps.Remove(map);
        }
    }
}