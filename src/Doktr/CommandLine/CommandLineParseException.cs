using System;

namespace Doktr.CommandLine
{
    public class CommandLineParseException : Exception
    {
        public CommandLineParseException(string message)
            : base(message) { }
    }
}