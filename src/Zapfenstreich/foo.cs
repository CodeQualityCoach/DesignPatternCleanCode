using NUnit.Framework;

namespace Zapfenstreich;

[TestFixture]
public class DisposalThings
{
    public class MakeMeDisposable : IDisposable
    {
        private readonly StreamWriter _stream;

        public MakeMeDisposable()
        {
            // open file with lock handle
            _stream = new StreamWriter(new MemoryStream());
        }

        public void WriteToStream(string message)
        {
            _stream.WriteLine(message);
        }

        public void Dispose()
        {
            _stream.Close();
        }
        ~MakeMeDisposable()
        {

        }
    }

    [Test]
    public void Foo_Awesome()
    {
        using var mmd = new MakeMeDisposable();
        mmd.WriteToStream("Hello World");
    }

    [Test]
    public void Foo()
    {
        var mmd = new MakeMeDisposable();

        try
        {
            mmd.WriteToStream("Hello World");
        }
        finally
        {
            mmd.Dispose();
        }
    }
}