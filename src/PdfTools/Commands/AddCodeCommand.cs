using System;

namespace PdfTools.Commands
{
    [CommandName("addcode")]
    public class AddCodeCommand : ICommand
    {
        public string Usage { get; } = @"usage: `pdftools addcode <input> <text> [output]`

Adds a QR Code with a <text> to the <input> pdf. If [output] is given, the pdf is stored as [output], otherwise it overrides <input>.";

        public bool CanExecute(object context)
        {
            // the context should be a string[], which are the program args without the command
            if (!(context is string[] args))
                return false;

            // and we expect two parameters (or an optional third one)
            return args.Length == 2 || args.Length == 3;
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
            // we don't change anything here. The goal is the command pattern.
            var enhancer = new PdfCodeEnhancer(args[0]);

            enhancer.AddTextAsCode(args[1]);

            if (args.Length == 4)
                enhancer.SaveAs(args[2]);
            else
                enhancer.SaveAs(args[0]);
        }
    }
}