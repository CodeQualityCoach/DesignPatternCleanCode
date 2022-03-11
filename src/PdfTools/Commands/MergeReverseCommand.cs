using System.IO.Abstractions;
using PdfTools.Handler;

namespace PdfTools.Commands
{
    [CommandName("mergescan")]
    public class MergeReverseCommand : ICommand
    {
        private readonly IDocumentHandlerFactory _handlerFactory;
        private readonly IFileSystem _fileSystem;
        private readonly ReverseCommand _reverseCommand;
        private readonly ShuffleCommand _shuffleCommand;

        public MergeReverseCommand(IDocumentHandlerFactory handlerFactory = null, IFileSystem fileSystem = null)
        {
            _handlerFactory = handlerFactory ?? new PdfHandlerFactory();
            _fileSystem = fileSystem ?? new FileSystem();
            _reverseCommand = new ReverseCommand(_handlerFactory, _fileSystem);
            _shuffleCommand = new ShuffleCommand(_handlerFactory, _fileSystem);
        }

        public string Usage { get; } = @"usage: `pdftools mergescan <output> <input 1> <input 2>`

Merges a two paged single side flipped scan. Order of pdf1 is '1, 3, 5, 7' and pdf2 is '8, 6, 4, 2'";

        public bool CanExecute(object context)
        {
            // the context should be a string[], which are the program args without the command
            if (!(context is string[] args))
                return false;

            // and we expect more than parameters (out, in, ...)
            return args.Length == 3;
        }

        public void Execute(object context)
        {
            var args = (string[])context;
            var tmp = _fileSystem.Path.GetTempFileName();
            _reverseCommand.Execute(new[] { tmp, args[2] });
            _shuffleCommand.Execute(new[] { args[0], args[1], tmp });
        }
    }
}