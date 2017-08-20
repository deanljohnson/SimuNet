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

        public ErrorCode Error { get; private set; }

        public void DoOp(OpCode code, int a, int b, out int c)
        {
            Error = ErrorCode.None;
            switch (code)
            {
                case OpCode.Add:
                    c = a + b;
                    break;
                case OpCode.Sub:
                    c = a - b;
                    break;
                case OpCode.Mul:
                    c = a * b;
                    break;
                case OpCode.Div:
                    if (b == 0)
                    {
                        Error = ErrorCode.DivisionByZero;
                        c = 0;
                        return;
                    }
                    c = a / b;
                    break;
                case OpCode.NoOp:
                    c = 0;
                    break;
                case OpCode.Load:
                case OpCode.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, "Given OpCode cannot be applied to these arguments");
            }
        }
    }
}
