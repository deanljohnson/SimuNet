using System;

namespace SimuNet
{
    /// <summary>
    /// Simulates an arithmetic logic unit (ALU). The ALU executes
    /// basic mathematic instructions such as addition and multiplication.
    /// </summary>
    public class ALU
    {
        public enum ErrorCode
        {
            /// <summary>
            /// No error occured.
            /// </summary>
            None,
            /// <summary>
            /// An attempt to divide by zero was made.
            /// </summary>
            DivisionByZero
        }

        [Flags]
        public enum Flags
        {
            None = 0,
            /// <summary>
            /// The result of the last operation was zero.
            /// </summary>
            Zero = 1 << 1,
            /// <summary>
            /// The result of the last operation was positive.
            /// </summary>
            Positive = 1 << 2,
            /// <summary>
            /// The result of the last operation was negative.
            /// </summary>
            Negative = 1 << 3
        }

        /// <summary>
        /// The error, if any, associated with the last invocation of <see cref="DoOp"/>.
        /// </summary>
        public ErrorCode Error { get; private set; }

        /// <summary>
        /// Flags providing additional information on the last invocation of <see cref="DoOp"/>.
        /// </summary>
        public Flags StatusFlags { get; private set; }

        public void DoOp(OpCode code, int a, int b, out int c, out int d)
        {
            Error = ErrorCode.None;
            d = 0;
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
                    d = a % b;
                    break;
                case OpCode.LeftShift:
                case OpCode.LeftShiftI:
                    c = a << b;
                    break;
                case OpCode.RightShift:
                case OpCode.RightShiftI:
                    c = a >> b;
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
