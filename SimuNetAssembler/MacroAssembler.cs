using System;
using System.Collections.Generic;

namespace SimuNetAssembler
{
    /// <summary>
    ///     Provides macro parsing and assembly functionality.
    /// </summary>
    internal class MacroAssembler
    {
        private readonly List<string> m_MacroSourceBuffer = new List<string>();
        private readonly Dictionary<string, Macro> m_Macros = new Dictionary<string, Macro>();
        private Macro m_CurrentMacro;

        /// <summary>
        ///     Whether or not the assembler is currently processing a macro.
        /// </summary>
        internal bool InMacro { get { return m_CurrentMacro != null; } }

        /// <summary>
        ///     Begins processing a macro. This should only be called with
        ///     lines for which <see cref="IsBeginMacro"/> returns true.
        /// </summary>
        /// <param name="line">The starting line of the macro containing the name and argument list.</param>
        /// <param name="lineNumber">The line number within the source at which this line occurs.</param>
        /// <exception cref="InvalidOperationException">A macro is already in progress.</exception>
        /// <exception cref="InvalidOperationException">A macro argument could not be parsed.</exception>
        internal void BeginMacro(string line, int lineNumber)
        {
            if (InMacro)
                throw new InvalidOperationException($"Nested macro around line {lineNumber}");

            string[] tokens = line.Split(' ');

            string[] arguments = new string[tokens.Length - 2];
            for (int i = 0; i < arguments.Length; i++)
            {
                string arg = tokens[i + 2];
                if (arg[0] != '$'
                    || !int.TryParse(arg.Substring(1), out int argIndex)
                    || argIndex != i + 1)
                    throw new InvalidOperationException($"Cannot parse parameter {i} of macro on line {lineNumber}");
                arguments[i] = arg;
            }

            m_CurrentMacro = new Macro()
            {
                Name = tokens[1],
                Arguments = arguments
            };
        }

        /// <summary>
        ///     Adds the given line to the active macros source.
        /// </summary>
        /// <param name="line">The source line to add.</param>
        internal void AddToMacroSource(string line)
        {
            m_MacroSourceBuffer.Add(line);
        }

        /// <summary>
        ///     Ends the current macro.
        /// </summary>
        /// <param name="lineNumber">The line number within the source at which this line occurs.</param>
        /// <exception cref="InvalidOperationException">A macro is not in progress.</exception>
        internal void EndMacro(int lineNumber)
        {
            if (!InMacro)
                throw new InvalidOperationException($"Invalid end of macro on line {lineNumber}");

            m_CurrentMacro.Instructions = m_MacroSourceBuffer.ToArray();
            m_MacroSourceBuffer.Clear();
            m_Macros.Add(m_CurrentMacro.Name, m_CurrentMacro);
            m_CurrentMacro = null;
        }

        /// <summary>
        ///     Get the macro of the given name.
        /// </summary>
        /// <param name="name">The name of the macro to search for.</param>
        /// <param name="macro">The macro of the given name.</param>
        /// <returns>Whether or not a macro could be found with the given name.</returns>
        internal bool TryGetMacro(string name, out Macro macro)
        {
            return m_Macros.TryGetValue(name, out macro);
        }

        /// <summary>
        ///     Determines whether or not the given line
        ///     is the start of a macro definition.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>Whether or not the line is the start of a macro definition.</returns>
        internal bool IsBeginMacro(string line)
        {
            return line.StartsWith("#begin");
        }

        /// <summary>
        ///     Determines whether or not the given line
        ///     is the end of a macro definition.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>Whether or not the line is the end of a macro definition.</returns>
        internal bool IsEndMacro(string line)
        {
            return line == "#end";
        }
    }
}
