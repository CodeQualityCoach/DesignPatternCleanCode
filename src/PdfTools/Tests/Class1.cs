using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace PdfTools.Tests
{
    [TestFixture]
    internal class PdfArchiverTests
    {
        // this is a real manual mock. But you need a mock for each test or case
        private class MyGeneratorMock : IBarcodeGenerator
        {
            public Bitmap CreateTextBarcode(string text)
            {
                return new Bitmap(1, 1);
            }
        }

        [Test]
        public void Foo()
        {
            const string url = "https://www.mclibre.org/descargar/docs/revistas/magpi-books/the-magpi-essentials-scratch-01-en-201606.pdf";

            // lets create a dummy response based on a local embedded pdf file
            File.Exists("Sample01.pdf").Should().BeTrue("File was not copied to output");
            var contents = File.ReadAllBytes("Sample01.pdf");
            var response = new HttpResponseMessage() { Content = new ByteArrayContent(contents) };

            // lets return our message as result from http client call
            var httpMock = Substitute.For<IHttpClient>();
            httpMock.GetAsync(url).Returns(Task.FromResult(response));

            var sut = new PdfArchiver(new MyGeneratorMock(), httpMock);

            sut.Archive(url);

            httpMock.Received(0).GetAsync(url);

            sut.SaveAs(@"C:\Workspaces\DOS2.pdf");

            Assert.IsTrue(File.Exists(@"C:\Workspaces\DOS2.pdf"));
            File.Exists(@"C:\Workspaces\DOS2.pdf").Should().BeTrue();
        }
    }
}
