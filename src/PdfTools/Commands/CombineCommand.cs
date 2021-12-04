using System;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfTools.Commands
{
    public class CombineCommand : ICommand
    {
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

            // step 1: creation of a document-object
            var document = new Document();
            //create newFileStream object which will be disposed at the end
            using (var newFileStream = new FileStream(outFile, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                var writer = new PdfCopy(document, newFileStream);

                // step 3: we open the document
                document.Open();

                foreach (var fileName in fileNames)
                {
                    // we create a reader for a certain document
                    var reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    reader.Close();
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            } //disposes the newFileStream object
        }
    }
}