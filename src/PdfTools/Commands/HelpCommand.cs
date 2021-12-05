using System;

namespace PdfTools.Commands
{
    [CommandName("help")]
    public class HelpCommand : ICommand
    {
        public string Usage { get; } = @"usage: `pdftools help`

Shows a list of all commands and their usage.";

        public bool CanExecute(object context)
        {
            // the context should be a string[], which are the program args without the command
            if (!(context is string[] args))
                return false;

            return true;
        }

        public void Execute(object context)
        {
            DoExecute((string[])context);
        }

        private static void DoExecute(string[] context)
        {
            var commands = CommandHelper.GetCommands();

            foreach (var command in commands)
            {
                Console.WriteLine($"### Command: **{command.GetName()}**");
                Console.WriteLine(command.Usage);
                Console.WriteLine();
            }
        }
    }
}