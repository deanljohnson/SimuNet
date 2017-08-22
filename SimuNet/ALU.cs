using System;

namespace SimuNet
{
    public class ALU
    {
        public enum ErrorCode
        {
            None,
            DivisionByZero
        }

        [Flags]
        public enum Flags
        {
            None = 0,
            Zero = 1 << 1,
            Positive = 1 << 2,
            Negative = 1 << 3
        }

        public ErrorCode Error { get; private set; }
        public Flags StatusFlags { get; private set; }

        public void DoOp(OpCode code, int a, int b, out int c)
        {
            Error = ErrorCode.None;
            switch (code)
            {
                case OpCode.Add:
                case OpCode.AddI:
                    c = a + b;
                    break;
                case OpCode.Sub:
                case OpCode.SubI:
                    c = a - b;
                    break;
                case OpCode.Mul:
                case OpCode.MulI:
                    c = a * b;
                    break;
                case OpCode.Div:
                case OpCode.DivI:
                    if (b == 0)
                    {
                        Error = ErrorCode.DivisionByZero;
                        c = 0;
                        return;
                    }
                    c = a / b;
                    break;
                case OpCode.Equal:
                    c = a == b ? 1 : 0;
                    break;
                case OpCode.NoOp:
                    c = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, "Given OpCode cannot be applied to these arguments");
            }

            StatusFlags = Flags.None;
            if (c == 0)
                StatusFlags |= Flags.Zero;
            if (c > 0)
                StatusFlags |= Flags.Positive;
            if (c < 0)
                StatusFlags |= Flags.Negative;
        }
    }
}
