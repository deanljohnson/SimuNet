using System;

namespace SimuNetAssembler
{
    /// <summary>
    /// Describes a macro defined in a program.
    /// </summary>
    internal class Macro
    {
        /// <summary>
        /// The name of the macro. This takes the place of an instruction OpCode.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The arguments to the macro. Any tokens matching these arguments will
        /// be replaced with the values specified in the source file.
        /// </summary>
        public string[] Arguments { get; set; }
        /// <summary>
        /// The instruction source code for the macro.
        /// </summary>
        public string[] Instructions { get; set; }

        /// <summary>
        ///     Returns the macro source code with the given
        ///     arguments substituted for macro placeholders.
        /// </summary>
        /// <param name="args">The arguments to substitute into the macro.</param>
        /// <returns>The macro source code with substituted arguments.</returns>
        /// <exception cref="InvalidOperationException">An incorrect number of arguments was passed.</exception>
        public string[] SubstituteArguments(Span<string> args)
        {
            if (args.Length != Arguments.Length)
                throw new InvalidOperationException($"Incorrect number of arguments for macro {Name}. Expected {Arguments.Length}, given {args.Length}");

            string[] source = new string[Instructions.Length];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = Instructions[i];
                for (int j = 0; j < Arguments.Length; j++)
                {
                    source[i] = source[i].Replace(Arguments[j], args[j]);
                }
            }
            return source;
        }
    }
}