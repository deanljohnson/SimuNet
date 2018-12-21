namespace SimuNet
{
    public enum OpCode
    {
        Error = -1,
        NoOp,
        Exit,

        Add,
        Sub,
        Mul,
        Div,

        AddI,
        SubI,
        MulI,
        DivI,

        LeftShift,
        RightShift,
        LeftShiftI,
        RightShiftI,

        Equal,

        Load,
        Move,

        Push,
        Pop,

        Jump,
        JumpRegister,
        BranchOnZero,
        BranchOnNotZero,
        BranchOnEqual,
        BranchOnNotEqual,
        BranchOnLessThan,
        BranchOnGreaterThan,
        BranchOnLessThanOrEqual,
        BranchOnGreaterThanOrEqual,

        PrintRegister
    }
}
