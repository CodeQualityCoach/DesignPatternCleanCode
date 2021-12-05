using System;
using System.IO;
using System.Net.Http;

namespace PdfTools.Commands
{
    [CommandName("download")]
    public class DownloadCommand : ICommand
    {
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
            var client = new HttpClient();
            var response = client.GetAsync(args[0]).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

            File.WriteAllBytes(args[1], pdf);
        }
    }
}