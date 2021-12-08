using System;
using PdfTools.Handler;

namespace PdfTools.Commands
{
    [CommandName("download")]
    public class DownloadCommand : ICommand
    {
        private readonly IDocumentHandlerFactory _handlerFactory;

        public DownloadCommand(IDocumentHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory ?? new PdfHandlerFactory();
        }

        public string Usage { get; } = @"usage: `pdftools download <url> <output>`

Downloads a pdf file <url> from the web and stores it locally as <output>.";

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
            using (var handler = _handlerFactory.Download(args[0]))
            {
                handler.SaveAs(args[1]);
            }
        }
    }
}