using System;
using System.Linq;
using PdfTools.Commands;

// ReSharper disable StringLiteralTypo

namespace PdfTools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Beware: there is one significant change: The commands only know the subset of args[] without the command name!
            var commandName = (args.Length > 0) ? args[0] : string.Empty;
            var commandContext = args.Skip(1).ToArray(); // we need to make this ToArray(), otherwise it is an IEnumerable

            // We simply create a dictionary, with the command name as key. Thanks to reflection and attributes.
            var availableCommands = CommandHelper.GetCommands().ToDictionary(x => x.GetName(), x => x);

            // now, we get the command instance
            if (!availableCommands.TryGetValue(commandName, out var commandInstance))
            {
                Console.WriteLine($"Cannot find command: '{commandName}'\r\n");
                commandInstance = new HelpCommand();
            }

            // and check if the command can be executed
            if (commandInstance.CanExecute(commandContext))
                commandInstance.Execute(commandContext);
            else
                Console.WriteLine(commandInstance.Usage);
        }
    }
}
