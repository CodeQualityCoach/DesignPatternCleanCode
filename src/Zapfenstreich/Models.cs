namespace Zapfenstreich;

public class Models
{
    public interface ILogger
    {
        void Log(string message);
    }

    private class EmptyLogger : ILogger
    {
        public void Log(string message)
        {
            // I do nothing because I am a null object implementation
        }
    }

    public class LoggerImpl1 : ILogger
    {
        public LoggerImpl1()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Console.WriteLine($"Constructor of {GetType().Name} with #{GetHashCode()}");
        }

        public void Log(string message)
        {
            Console.WriteLine($"[#{GetHashCode()}] Log:{message}");
        }
    }

    public interface IMailer
    {
        void SendMail(string message);
    }

    public class OutlookMailer : IMailer
    {
        private readonly ILogger _logger;

        public OutlookMailer(ILogger logger)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Console.WriteLine($"Constructor of {GetType().Name} with #{GetHashCode()}");

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Console.WriteLine($"Constructor of {GetType().Name} with ILogger #{_logger.GetHashCode()}");
        }

        public ILogger Logger => _logger;

        public void SendMail(string message)
        {
            Console.WriteLine($"Email '{message}' was sent to /dev/null");
        }
    }

    private class EmptyMailer : IMailer
    {
        public void SendMail(string message)
        {
            // I do nothing because I am a null object implementation
        }
    }
}