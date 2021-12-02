using NUnit.Framework;
using StructureMap;

namespace Zapfenstreich.DI
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class Use_Di
    {
        [Test]
        public void Di_As_Designed()
        {
            var container = new Container(_ =>
            {
                _.For<Models.ILogger>().Use<Models.LoggerImpl1>();
            });

            var logger = container.GetInstance<Models.ILogger>();

            logger.Log("Hello World");
        }

        [Test]
        public void Di_As_SingletonService()
        {
            var container = new Container(_ =>
            {
                _.For<Models.ILogger>().Use<Models.LoggerImpl1>().Singleton();
            });

            // lets create some more logger
            var loggers = new List<Models.ILogger>();
            for (var i = 0; i < 10; i++)
                loggers.Add(container.GetInstance<Models.ILogger>());

            loggers.ForEach(x => x.Log("Hello World"));
        }
    }
}