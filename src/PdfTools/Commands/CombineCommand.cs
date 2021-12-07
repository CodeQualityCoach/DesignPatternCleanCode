using System;
using System.Linq;
using PdfTools.Handler;

namespace PdfTools.Commands
{
    [CommandName("combine")]
    public class CombineCommand : ICommand
    {
        public string Usage { get; } = @"usage: `pdftools combine <output> <input 1> <input 2> [input 3..n]`

Combines two or more input files <input n> into a single pdf <output>";

        public bool CanExecute(object context)
        {
            // the context should be a string[], which are the program args without the command
            if (!(context is string[] args))
                return false;

            // and we expect more than parameters (out, in, in, ...)
            return args.Length > 2;
        }

        public void Execute(object context)
        {
            // I like to double check if the context is valid
            if (!CanExecute(context))
                throw new ArgumentException("command cannot be executed");

            // I prefer to separate the "interface" needs from the implementation
            // In our class, we already verified, that the context is a string[], so
            // we can safely cast this. This is another reason for the check above.
            DoExecute((string[])context);
        }

        private void DoExecute(string[] args)
        {
            var fileNames = args.Skip(1).ToArray();
            var outFile = args[0];

            using (var handler = new PdfHandler())
            {
                handler.Open(fileNames.First());

                // let us append all the other files to the first file
                handler.Append(fileNames.Skip(1).ToArray());

                handler.SaveAs(outFile);
            }
        }
    }
}