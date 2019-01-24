using System;

namespace SimuNet
{
    public class MemoryMap
    {
        public int BaseAddress { get; }
        public int Length { get; }

        public delegate void SetDelegate(int addr, int value);
        public delegate int GetDelegate(int addr);

        public SetDelegate MemorySet;
        public GetDelegate MemoryGet;

        public MemoryMap(int baseAddress, int length)
        {
            if (baseAddress < 0)
                throw new ArgumentOutOfRangeException(nameof(baseAddress));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            BaseAddress = baseAddress;
            Length = length;
        }

        public void Set(int addr, int value)
        {
            MemorySet?.Invoke(addr, value);
        }

        public int Get(int addr)
        {
            return MemoryGet?.Invoke(addr) ?? 0;
        }
    }
}