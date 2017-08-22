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

        Equal,

        Load,
        Move,

        Jump,
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
