
namespace ApiCatalog.Logging;

public class Logger : ILogger
{
    readonly string loggerName;
    readonly LoggingConfiguration loggingConfig;

    public Logger(string name, LoggingConfiguration config)
    {
        this.loggerName = name;
        this.loggingConfig = config;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggingConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel.ToString()}:{eventId.Id} - {formatter(state, exception)}";
        WriteLog(message);
    }

    private void WriteLog(string message)
    {
        string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        using (var sw = new StreamWriter(logPath, true))
        {
            try
            {
                sw.WriteLine(message);
                sw.Close();

            }
            catch
            {
                throw;
            }
        }
    }
}
