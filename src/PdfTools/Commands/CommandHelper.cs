using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PdfTools.Commands
{
    public static class CommandHelper
    {
        // This is a single point to get all commands. This checks for all required conditions on a type,
        // not only that the type is an ICommand.
        public static IEnumerable<ICommand> GetCommands()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(ICommand)) && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as ICommand);
        }

        // this makes it easier to read if code needs to get the contents of the CommandName attribute.
        public static string GetName(this ICommand command)
        {
            var att = command.GetType().GetCustomAttribute<CommandNameAttribute>();
            return att?.CommandName;
        }
    }
}