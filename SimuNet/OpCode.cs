namespace SimuNet
{
    public enum OpCode
    {
        Error = -1,
        NoOp,
        Exit,

        // Binary Registers
        Add,
        Sub,
        Mul,
        Div,

        // One reg, one immediate
        Load,

        // One immediate
        Jump
    }
}
