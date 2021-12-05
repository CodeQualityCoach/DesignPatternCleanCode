using FluentAssertions;
using NUnit.Framework;
using PdfTools.Services;

namespace PdfTools.Tests.Services
{
    [TestFixture]
    public class QrCoderServiceTest
    {
        [Test(Description = "I like this Be_Creatable() test to verify e.g. the interface or constructor logic")]
        public void Be_Creatable()
        {
            var sut = new QrCoderService();

            sut.Should().BeAssignableTo<IOverlayImageService>();
        }

        [Test(Description = "We check on the happy path to verify the result if everything is correct")]
        public void Create_An_Image_For_Text()
        {
            var sut = new QrCoderService();
            var actual = sut.CreateOverlayImage("this is a text");

            actual.Should().NotBeNull();
            actual.Width.Should().Be(66); // for simplicity we just check on the returned size
            actual.Height.Should().Be(66);
        }

        [Test(Description = "We check the error path and not we need to define the behaviour on all error path")]
        public void Create_An_Image_For_Text_With_Null_Text()
        {
            var sut = new QrCoderService();
            var actual = sut.CreateOverlayImage((string)null);

            // I define that in an error path for a null string, I just return an empty QR code
            actual.Should().NotBeNull();
            actual.Width.Should().Be(58); // an empty QR code is much smaller
            actual.Height.Should().Be(58);
        }
    }
}
