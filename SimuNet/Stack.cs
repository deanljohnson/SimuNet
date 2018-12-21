using System;

namespace SimuNet
{
    /// <summary>
    /// Simulates a programs stack space.
    /// </summary>
    public class Stack
    {
        private readonly int[] m_Stack;

        public int this[int address]
        {
            get
            {
                if (address < 0)
                    throw new InvalidOperationException("Stack Underflow");

                return m_Stack[address];
            }
            set
            {
                if (address >= m_Stack.Length)
                    throw new InvalidOperationException("Stack Overflow");
                m_Stack[address] = value;
            }
        }

        public Stack(int size)
        {
            m_Stack = new int[size];
        }
    }
}
