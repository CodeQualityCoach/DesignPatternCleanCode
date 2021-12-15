using System;
using FluentAssertions;
using NUnit.Framework;
using PdfTools.Commands;

namespace PdfTools.Tests
{
    [TestFixture]
    internal class ProgramTest
    {
        [Test]
        public void Get_Help()
        {
            var args = new[] { "help" };

            // this class uses the dispose pattern to override the default output
            // AND reset it after the test (using) is done
            using (var consoleOutput = new ConsoleOutputForTest())
            {
                Program.Main(args);

                consoleOutput.GetOutput().Should().NotBeNull();
                consoleOutput.GetOutput().Should().StartWith("### Command:");
            }

        }

        [Test]
        public void Get_Help_If_No_Parameter()
        {
            var args = Array.Empty<string>();

            // this class uses the dispose pattern to override the default output
            // AND reset it after the test (using) is done
            using (var consoleOutput = new ConsoleOutputForTest())
            {
                Program.Main(args);

                consoleOutput.GetOutput().Should().NotBeNull();
                consoleOutput.GetOutput().Should().StartWith("Cannot find command: ''");
            }
        }

        [Test]
        public void Get_Usage_If_Cannot_Execute()
        {
            var args = new[] { "create", "One_param_but_two_required" };

            // this class uses the dispose pattern to override the default output
            // AND reset it after the test (using) is done
            using (var consoleOutput = new ConsoleOutputForTest())
            {
                Program.Main(args);

                consoleOutput.GetOutput().Should().NotBeNull();
                consoleOutput.GetOutput().Should().StartWith(new CreateCommand().Usage);
            }
        }
    }
}
