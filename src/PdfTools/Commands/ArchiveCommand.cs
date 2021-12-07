using System;
using PdfTools.Handler;

namespace PdfTools.Commands
{
    [CommandName("archive")]
    public class ArchiveCommand : ICommand
    {
        public string Usage { get; } = @"usage: `pdftools archive <url> <output>`

Downloads a pdf file from an <url> and adds the url as a barcode on the first page. The output pdf is stored as <output>";

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
            using (var handler = new PdfHandler())
            {
                handler.Download(args[0]);
                handler.AddOverlayImage(args[0]);
                handler.SaveAs(args[1]);
            }
        }
    }
}