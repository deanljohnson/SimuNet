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

        Equal,

        Load,

        Jump,
        BranchOnZero,
        BranchOnNotZero,
        BranchOnEqual
    }
}
