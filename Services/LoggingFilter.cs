using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalog.Services;

public class LoggingFilter : IActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"###{DateTime.Now.ToString("dd/MM/yyyy HH:mm")} - ModelState:{context.ModelState.IsValid}###");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"###{DateTime.Now.ToString("dd/MM/yyyy HH:mm")} - Status Code:{context.HttpContext.Response.StatusCode}###");
    }
}
