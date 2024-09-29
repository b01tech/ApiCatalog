using System.Collections.Concurrent;

namespace ApiCatalog.Logging;

public class LoggingProvider : ILoggerProvider
{
    readonly LoggingConfiguration loggerConfig;
    readonly ConcurrentDictionary<string, Logger> loggers = new ConcurrentDictionary<string, Logger>();

    public LoggingProvider(LoggingConfiguration loggerConfig)
    {
        this.loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new Logger(name, loggerConfig));
    }

    public void Dispose()
    {
       loggers.Clear();
    }
}
