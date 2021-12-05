using System;

namespace PdfTools.Commands
{
    public class CommandNameAttribute : Attribute
    {
        public string CommandName { get; }

        public CommandNameAttribute(string commandName)
        {
            CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
        }
    }
}