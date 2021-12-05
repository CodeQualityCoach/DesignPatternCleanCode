using System;
using System.IO;
using FSharp.Markdown;
using FSharp.Markdown.Pdf;

namespace PdfTools.Commands
{
    [CommandName("create")]
    public class CreateCommand : ICommand
    {
        public string Usage { get; } = @"usage: `pdftools create <markdown> <output>`

Converts an input file <markdown> in markdown format into a pdf and stores it as  <output>.";

        public bool CanExecute(object context)
        {
            // the context should be a string[], which are the program args without the command
            if (!(context is string[] args))
                return false;

            // and we expect two parameters
            return args.Length == 2;
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
            var inFile = args[0];
            var outFile = args[1];

            var mdText = File.ReadAllText(inFile);
            var mdDoc = Markdown.Parse(mdText);

            MarkdownPdf.Write(mdDoc, outFile);
        }
    }
}