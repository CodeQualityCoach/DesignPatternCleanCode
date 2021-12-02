using NUnit.Framework;
using StructureMap;

namespace Zapfenstreich.DI;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class Use_Di_As_ServiceLocator
{
    public class DiServiceLocator
    {
        public static IContainer Container { private get; set; } = null!;

        public static Models.ILogger GetLogger()
        {
            return Container.GetInstance<Models.ILogger>();
        }
    }

    [Test]
    public void Make_A_Service_Locator()
    {
        var container = new Container(_ =>
        {
            _.For<Models.ILogger>().Use<Models.LoggerImpl1>();
        });

        DiServiceLocator.Container = container;

        var loggers = new List<Models.ILogger>();
        for (var i = 0; i < 10; i++)
            loggers.Add(DiServiceLocator.GetLogger());

        loggers.ForEach(x => x.Log("Hello World"));
    }
}