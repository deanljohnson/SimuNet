using System;

namespace SimuNet
{
    public class ALU
    {
        public enum OpCode
        {
            AddI,
            SubI,
            MulI,
            DivI
        }

        public enum ErrorCode
        {
            None,
            DivisionByZero
        }

        public ErrorCode Error { get; private set; }

        public void DoOp(OpCode code, int a, int b, out int c)
        {
            Error = ErrorCode.None;
            switch (code)
            {
                case OpCode.AddI:
                    c = a + b;
                    break;
                case OpCode.SubI:
                    c = a - b;
                    break;
                case OpCode.MulI:
                    c = a * b;
                    break;
                case OpCode.DivI:
                    if (b == 0)
                    {
                        Error = ErrorCode.DivisionByZero;
                        c = 0;
                        return;
                    }
                    c = a / b;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, "Given OpCode cannot be applied to these arguments");
            }
        }
    }
}
