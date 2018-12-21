namespace SimuNet
{
    public class Register
    {
        public string Name { get; }
        public int Value { get; set; } = 0;

        public Register(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
