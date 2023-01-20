using OpenTelemetry;
using OpenTelemetry.Logs;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Logging;

public class OpenTelemetryLogGrayLogProcessor
    : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord data)
    {
        // Aditional log here
        base.OnEnd(data);
    }
}
