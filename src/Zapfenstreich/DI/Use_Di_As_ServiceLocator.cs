using NUnit.Framework;
using StructureMap;

namespace Zapfenstreich.DI;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class Use_Di_As_ServiceLocator
{
    public class DiServiceLocator
    {
        public static IContainer Container { private get; set; } = new Container(_ =>
        {
            _.For<Models.ILogger>().Use<Models.LoggerImpl1>();
        });

        public static Models.ILogger GetLogger()
        {
            return Container.GetInstance<Models.ILogger>();
        }

        public static T Get<T>()
        {
            return Container.GetInstance<T>();
        }
    }

    [Test]
    public void Make_A_Service_Locator()
    {
        var loggers = new List<Models.ILogger>();
        for (var i = 0; i < 10; i++)
            loggers.Add(DiServiceLocator.GetLogger());

        loggers.ForEach(x => x.Log("Hello World"));
    }
}