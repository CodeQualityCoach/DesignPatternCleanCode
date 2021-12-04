using System.Reflection.Metadata.Ecma335;
using NUnit.Framework;

namespace Zapfenstreich.DI
{
    [TestFixture]
    public class LazyAndFunc
    {
        [Test]
        public void DoIt()
        {
            Func<Models.ILogger> loggerFactory1 = CreateLogger;
            Func<Models.ILogger> loggerFactory2 = () => { return new Models.LoggerImpl1(); };
            Func<Models.ILogger> loggerFactory3 = () => new Models.LoggerImpl1();

            loggerFactory1().Log("Hello World");

            LogMessage(loggerFactory1, "Hello2 World");

            LogMessageWithFactory(new LoggerFactory(), "Hello2 World");
        }

        [Test]
        public void SingletonLogger()
        {
            Lazy<Models.ILogger> singletonLogger = new Lazy<Models.ILogger>(CreateLogger);

            singletonLogger.Value.Log("Hello World");
            singletonLogger.Value.Log("Hello World");
            singletonLogger.Value.Log("Hello World");
        }

        private void LogMessage(Func<Models.ILogger> loggerFactory, string message)
        {
            var loggerInstance = loggerFactory();

            loggerInstance.Log(message);
        }

        private void LogMessageWithFactory(ILoggerFactory loggerFactory, string message)
        {
            var loggerInstance = loggerFactory.CreateLogger();

            loggerInstance.Log(message);
        }

        public Models.ILogger CreateLogger()
        {
            return new Models.LoggerImpl1();
        }

        public interface ILoggerFactory
        {
            Models.ILogger CreateLogger();
        }

        private class LoggerFactory : ILoggerFactory
        {
            public Models.ILogger CreateLogger()
            {
                return new Models.LoggerImpl1();
            }
        }
    }
}