using System;
using System.IO;

namespace PdfTools.Tests
{
    // If you find some interesting code on the internet, put the link into your code.
    // This help others why you solved your problem this way and more important: Let
    // others document your code
    // cf: https://www.codeproject.com/Articles/501610/Getting-Console-Output-Within-A-Unit-Test
    public class ConsoleOutputForTest : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public ConsoleOutputForTest()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}