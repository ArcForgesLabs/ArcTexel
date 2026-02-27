using ArcTexel.Extensions.CommonApi.Logging;
using ArcTexel.Extensions.Sdk.Bridge;

namespace ArcTexel.Extensions.Sdk.Api.Logging;

public class Logger : ILogger
{
    public void Log(string message)
    {
        InvokeApiLog(message);
    }

    public void LogError(string message)
    {
        InvokeApiLog(message);
    }

    public void LogWarning(string message)
    {
        InvokeApiLog(message);
    }

    private void InvokeApiLog(string message)
    {
        Native.log_message(message);
    }
}
