namespace ApiCatalog.Logging;

public class LoggingConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Debug;
    public int EventId {  get; set; } = 0;
}
