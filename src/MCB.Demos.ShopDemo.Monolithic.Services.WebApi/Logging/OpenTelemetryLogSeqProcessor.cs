using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using OpenTelemetry;
using OpenTelemetry.Logs;
using Serilog;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Logging;

public class OpenTelemetryLogSeqProcessor
    : BaseProcessor<LogRecord>
{
    // Fields
    private const string MESSAGE_TEMPLATE = "Timestamp: {Timestamp} TraceId: {TraceId} SpanId: {SpanId} TraceFlags: {TraceFlags} TraceState: {TraceState} CategoryName: {CategoryName} LogLevel: {LogLevel} EventId: {EventId} FormattedMessage: {FormattedMessage} State: {State} StateValues: {@StateValues} Exception: {Exception}";

    // Constructors
    public OpenTelemetryLogSeqProcessor(AppSettings appSettings)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty(name: nameof(Environment.MachineName), value: Environment.MachineName)
            .Enrich.WithProperty(name: nameof(appSettings.ApplicationName), value: appSettings.ApplicationName)
            .Enrich.WithProperty(name: nameof(appSettings.ApplicationVersion), value: appSettings.ApplicationVersion)
            .Enrich.WithProperty(name: nameof(Environment.OSVersion), value: Environment.OSVersion)
            .WriteTo.Seq(
                serverUrl: appSettings.Seq.Url,
                apiKey: appSettings.Seq.ApiKey
            )
            .CreateLogger();
    }

    // Public Methods
    public override void OnEnd(LogRecord data)
    {
        switch (data.LogLevel)
        {
            case LogLevel.Trace:
                Log.Logger.Verbose(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
            case LogLevel.Debug:
                Log.Logger.Debug(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
            case LogLevel.Information:
                Log.Logger.Information(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
            case LogLevel.Warning:
                Log.Logger.Warning(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
            case LogLevel.Error:
                Log.Logger.Error(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
            case LogLevel.Critical:
                Log.Logger.Fatal(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
            default:
                Log.Logger.Information(MESSAGE_TEMPLATE, data.Timestamp, data.TraceId, data.SpanId, data.TraceFlags, data.TraceState, data.CategoryName, data.LogLevel, data.EventId, data.FormattedMessage, data.State, data.StateValues, data.Exception);
                break;
        }

        base.OnEnd(data);
    }
}
