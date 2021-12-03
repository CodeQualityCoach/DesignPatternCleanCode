using NUnit.Framework;
using StructureMap;
using Zapfenstreich;

namespace DiTryouts
{
    public class StrategyResolver<T> : IStrategyResolver<T> where T : class
    {
        private readonly GlobalState _setting;
        private readonly IEnumerable<T> _registeredClasses;

        // it is better to add the structuremap context so an instance is not created before and request by name
        public StrategyResolver(GlobalState setting, IEnumerable<T> registeredClasses)
        {
            _setting = setting ?? throw new ArgumentNullException(nameof(setting));
            _registeredClasses = registeredClasses ?? throw new ArgumentNullException(nameof(registeredClasses));
        }

        public T Resolve()
        {
            var interfaceName = typeof(T).Name;

            var settingProperty = typeof(GlobalState).GetProperty(interfaceName) ?? throw new Exception($"A configuration for interface {interfaceName} could not be found on settings class {nameof(GlobalState)}");
            var setting = settingProperty.GetValue(_setting) as string;

            var instance = _registeredClasses.SingleOrDefault(x => x.GetType().Name == setting) ?? throw new Exception($"Configured class {setting} for interface {interfaceName} was not found in registered classes");

            return instance;
        }
    }

    [TestFixture]
    public class StrategyResolverTests
    {
        [Test]
        public void Resolve_Correct_Strategy()
        {
            var container = new Container(_ =>
            {
                _.For<Models.IMailer>().Use<Models.OutlookMailer>();
                _.For<Models.IMailer>().Use<Models.EmptyMailer>();
                _.For(typeof(IStrategyResolver<>)).Use(typeof(StrategyResolver<>));
            });

            var resolver = container.GetInstance<IStrategyResolver<Models.IMailer>>();
            var mailer = resolver.Resolve();
            mailer.SendMail("Hello World");


            var state = container.GetInstance<GlobalState>();
            state.IMailer = nameof(Models.EmptyMailer);
            mailer.SendMail("Hello World");

        }
    }
}