using NUnit.Framework;
using StructureMap;
using Zapfenstreich;

namespace DiTryouts;

[TestFixture]
public class StrategyResolverTests
{
    [Test]
    public void Resolve_Correct_Strategy()
    {
        // something is missing. How to behave?
        var container = new Container(_ =>
        {
            _.For<Models.ILogger>().Use<Models.EmptyLogger>();
            _.For<Models.IMailer>().Use<Models.EmptyMailer>();
            _.For<Models.IMailer>().Use<Models.OutlookMailer>();
            _.For(typeof(IStrategyResolver<>)).Use(typeof(StrategyResolver<>));
        });

        var state = container.GetInstance<GlobalState>();
        state.IMailer ="OutlookMailer";

        var resolver = container.GetInstance<IStrategyResolver<Models.IMailer>>();
        var mailer = resolver.Resolve();
        mailer.SendMail("Hello World");
        
        state.IMailer = nameof(Models.EmptyMailer);
        mailer.SendMail("Hello World");
    }
}