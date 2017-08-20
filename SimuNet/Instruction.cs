namespace SimuNet
{
    public class Instruction
    {
        public OpCode Code { get; }
        public Register A { get; }
        public Register B { get; }
        public Register C { get; }

        public int Immediate1 { get; }

        private Instruction(OpCode opCode, Register a = null, Register b = null, Register c = null)
        {
            Code = opCode;
            A = a;
            B = b;
            C = c;
            Immediate1 = 0;
        }

        private Instruction(OpCode opCode, Register a, Register b, int immediate1)
        {
            Code = opCode;
            A = a;
            B = b;
            C = null;
            Immediate1 = immediate1;
        }

        private Instruction(OpCode opCode, Register a, int immediate1)
        {
            Code = opCode;
            A = a;
            B = null;
            C = null;
            Immediate1 = immediate1;
        }

        private Instruction(OpCode opCode, int immediate1)
        {
            Code = opCode;
            A = null;
            B = null;
            C = null;
            Immediate1 = immediate1;
        }

        public static Instruction Add(Register a, Register b, Register c)
        {
            return new Instruction(OpCode.Add, a, b, c);
        }

        public static Instruction Sub(Register a, Register b, Register c)
        {
            return new Instruction(OpCode.Sub, a, b, c);
        }

        public static Instruction Mul(Register a, Register b, Register c)
        {
            return new Instruction(OpCode.Mul, a, b, c);
        }

        public static Instruction Div(Register a, Register b, Register c)
        {
            return new Instruction(OpCode.Div, a, b, c);
        }

        public static Instruction Load(Register a, int b)
        {
            return new Instruction(OpCode.Load, a, b);
        }

        public static Instruction Equal(Register a, Register b, Register c)
        {
            return new Instruction(OpCode.Equal, a, b, c);
        }

        public static Instruction Jump(int a)
        {
            return new Instruction(OpCode.Jump, a);
        }

        public static Instruction BranchOnZero(Register a, int target)
        {
            return new Instruction(OpCode.BranchOnZero, a, target);
        }

        public static Instruction BranchOnNotZero(Register a, int target)
        {
            return new Instruction(OpCode.BranchOnNotZero, a, target);
        }

        public static Instruction BranchOnEqual(Register a, Register b, int target)
        {
            return new Instruction(OpCode.BranchOnEqual, a, b, target);
        }

        public static Instruction NoOp()
        {
            return new Instruction(OpCode.NoOp);
        }

        public static Instruction Error()
        {
            return new Instruction(OpCode.Error);
        }

        public static Instruction Exit()
        {
            return new Instruction(OpCode.Exit);
        }
    }
}
