namespace SimuNet
{
    /// <summary>
    /// A single instruction CPU instruction.
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// The operation code of this instruction.
        /// </summary>
        public OpCode Code { get; }
        /// <summary>
        /// The first register used by this instruction. May be null.
        /// </summary>
        public Register A { get; }
        /// <summary>
        /// The second register used by this instruction. May be null.
        /// </summary>
        public Register B { get; }
        /// <summary>
        /// The third register used by this instruction. May be null.
        /// </summary>
        public Register C { get; }
        /// <summary>
        /// The 'immediate' value loaded with this instruction. Immediate values
        /// are integer constants written directly in the assembly code.
        /// </summary>
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

        public static Instruction AddI(Register a, int immediate, Register target)
        {
            return new Instruction(OpCode.AddI, a, target, immediate);
        }

        public static Instruction SubI(Register a, int immediate, Register target)
        {
            return new Instruction(OpCode.SubI, a, target, immediate);
        }

        public static Instruction MulI(Register a, int immediate, Register target)
        {
            return new Instruction(OpCode.MulI, a, target, immediate);
        }

        public static Instruction DivI(Register a, int immediate, Register target)
        {
            return new Instruction(OpCode.DivI, a, target, immediate);
        }

        public static Instruction Load(Register a, int b)
        {
            return new Instruction(OpCode.Load, a, b);
        }

        public static Instruction Move(Register a, Register b)
        {
            return new Instruction(OpCode.Move, a, b);
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

        public static Instruction BranchOnNotEqual(Register a, Register b, int target)
        {
            return new Instruction(OpCode.BranchOnNotEqual, a, b, target);
        }

        public static Instruction BranchOnLessThan(Register a, Register b, int target)
        {
            return new Instruction(OpCode.BranchOnLessThan, a, b, target);
        }

        public static Instruction BranchOnGreaterThan(Register a, Register b, int target)
        {
            return new Instruction(OpCode.BranchOnGreaterThan, a, b, target);
        }

        public static Instruction BranchOnLessThanOrEqual(Register a, Register b, int target)
        {
            return new Instruction(OpCode.BranchOnLessThanOrEqual, a, b, target);
        }

        public static Instruction BranchOnGreaterThanOrEqual(Register a, Register b, int target)
        {
            return new Instruction(OpCode.BranchOnGreaterThanOrEqual, a, b, target);
        }

        public static Instruction PrintRegister(Register a)
        {
            return new Instruction(OpCode.PrintRegister, a);
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
