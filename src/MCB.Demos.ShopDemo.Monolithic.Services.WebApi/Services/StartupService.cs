using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.RabbitMq.Connection.Interfaces;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models.Enums;
using MCB.Core.Infra.CrossCutting.RabbitMq.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Middlewares;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services.Interfaces;
using Npgsql;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services;

public class StartupService
    : IStartupService
{
    private static bool _isConfigured;
    private static bool _isStarted;

    private readonly AppSettings _appSettings;
    private readonly IPostgreSqlResiliencePolicy _postgreSqlResiliencePolicy;
    private readonly IRedisResiliencePolicy _redisResiliencePolicy;
    private readonly IRabbitMqResiliencePolicy _rabbitMqResiliencePolicy;
    private readonly IMetricsManager _metricsManager;
    private readonly IRabbitMqConnection _rabbitMqConnection;
    private readonly IMcbFeatureFlagManager _mcbFeatureFlagManager;

    // Properties
    public bool HasStarted { get; private set; }

    // Constructors
    public StartupService(
        AppSettings appSettings,
        IPostgreSqlResiliencePolicy postgreSqlResiliencePolicy,
        IRedisResiliencePolicy redisResiliencePolicy,
        IRabbitMqResiliencePolicy rabbitMqResiliencePolicy,
        IMetricsManager metricsManager,
        IRabbitMqConnection rabbitMqConnection,
        IMcbFeatureFlagManager mcbFeatureFlagManager
    )
    {
        _appSettings = appSettings;
        _postgreSqlResiliencePolicy = postgreSqlResiliencePolicy;
        _redisResiliencePolicy = redisResiliencePolicy;
        _rabbitMqResiliencePolicy = rabbitMqResiliencePolicy;
        _metricsManager = metricsManager;
        _rabbitMqConnection = rabbitMqConnection;
        _mcbFeatureFlagManager = mcbFeatureFlagManager;
    }

    // Public Methods
    public async Task<(bool Success, string[]? Messages)> TryStartupApplicationAsync(CancellationToken cancellationToken)
    {
        var result = default((bool Success, string[]? Messages));

        if (!_isConfigured)
        {
            // Configure resilience policies
            _postgreSqlResiliencePolicy.Configure(() => new ResiliencePolicyConfig
            {
                // Identification
                Name = _appSettings.PostgreSql.ResiliencePolicy.Name,
                // Retry
                RetryMaxAttemptCount = _appSettings.PostgreSql.ResiliencePolicy.RetryMaxAttemptCount,
                RetryAttemptWaitingTimeFunction = (attempt) => TimeSpan.FromMilliseconds(_appSettings.PostgreSql.ResiliencePolicy.RetryAttemptWaitingTimeMilliseconds * attempt),
                OnRetryAditionalHandler = null,
                // Circuit Breaker
                CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(_appSettings.PostgreSql.ResiliencePolicy.CircuitBreakerWaitingTimeSeconds),
                OnCircuitBreakerHalfOpenAditionalHandler = null,
                OnCircuitBreakerOpenAditionalHandler = null,
                OnCircuitBreakerCloseAditionalHandler = null,
                // Exceptions
                ExceptionHandleConfigArray = new[] {
                    new Func<Exception, bool>(ex => ex.GetType() == typeof(NpgsqlException) || ex.GetType() == typeof(PostgresException))
                }
            });

            _redisResiliencePolicy.Configure(() => new ResiliencePolicyConfig
            {
                // Identification
                Name = _appSettings.Redis.ResiliencePolicy.Name,
                // Retry
                RetryMaxAttemptCount = _appSettings.Redis.ResiliencePolicy.RetryMaxAttemptCount,
                RetryAttemptWaitingTimeFunction = (attempt) => TimeSpan.FromMilliseconds(_appSettings.Redis.ResiliencePolicy.RetryAttemptWaitingTimeMilliseconds * attempt),
                OnRetryAditionalHandler = null,
                // Circuit Breaker
                CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(_appSettings.Redis.ResiliencePolicy.CircuitBreakerWaitingTimeSeconds),
                OnCircuitBreakerHalfOpenAditionalHandler = null,
                OnCircuitBreakerOpenAditionalHandler = null,
                OnCircuitBreakerCloseAditionalHandler = null,
                // Exceptions
                ExceptionHandleConfigArray = new[] {
                    new Func<Exception, bool>(ex => ex.GetType() == typeof(StackExchange.Redis.RedisException)),
                }
            });

            _rabbitMqResiliencePolicy.Configure(() => new ResiliencePolicyConfig
            {
                // Identification
                Name = _appSettings.RabbitMq.ResiliencePolicy.Name,
                // Retry
                RetryMaxAttemptCount = _appSettings.RabbitMq.ResiliencePolicy.RetryMaxAttemptCount,
                RetryAttemptWaitingTimeFunction = (attempt) => TimeSpan.FromMilliseconds(_appSettings.RabbitMq.ResiliencePolicy.RetryAttemptWaitingTimeMilliseconds * attempt),
                OnRetryAditionalHandler = null,
                // Circuit Breaker
                CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(_appSettings.RabbitMq.ResiliencePolicy.CircuitBreakerWaitingTimeSeconds),
                OnCircuitBreakerHalfOpenAditionalHandler = null,
                OnCircuitBreakerOpenAditionalHandler = null,
                OnCircuitBreakerCloseAditionalHandler = null,
                // Exceptions
                ExceptionHandleConfigArray = new[] {
                    new Func<Exception, bool>(ex => true),
                }
            });

            ResiliencePoliciesController.SetResiliencePolicyCollection(
                new IResiliencePolicy[]
                {
                    _postgreSqlResiliencePolicy,
                    _redisResiliencePolicy,
                    _rabbitMqResiliencePolicy
                }
            );

            // Configure Metrics
            _metricsManager.CreateCounter<int>(name: Metrics.Constants.REQUESTS_COUNTER_NAME);
            _metricsManager.CreateCounter<int>(name: Metrics.Constants.EXCEPTIONS_COUNTER_NAME);

            _metricsManager.CreateHistogram<int>(name: Metrics.Constants.REQUESTS_HISTOGRAM_NAME);

            McbGlobalExceptionMiddleware.SetMetricsManager(_metricsManager, Metrics.Constants.EXCEPTIONS_COUNTER_NAME);
            McbRequestCounterMetricMiddleware.SetMetricsManager(_metricsManager, Metrics.Constants.REQUESTS_COUNTER_NAME);

            _isConfigured = true;
        }

        if(!_isStarted)
        {
            var messageCollection = new List<string>();

            // Startup RabbitMQ
            _rabbitMqConnection.OpenConnection();
            if (_rabbitMqConnection.IsOpen)
            {
                _rabbitMqConnection.ExchangeDeclare(
                    new RabbitMqExchangeConfig(
                        ExchangeName: _appSettings.RabbitMq.EventsExchange.Name,
                        ExchangeType: RabbitMqExchangeType.Header,
                        Durable: _appSettings.RabbitMq.EventsExchange.Durable,
                        AutoDelete: _appSettings.RabbitMq.EventsExchange.AutoDelete,
                        Arguments: null
                    )
                );
            }
            else
                messageCollection.Add("Fail on startup RabbitMQ");

            if(messageCollection.Count == 0)
                _isStarted = true;
            else
                result.Messages = messageCollection.ToArray();

            // Startup Feature Flags
            await _mcbFeatureFlagManager.InitAsync(cancellationToken: default);
        }

        result.Success = HasStarted = _isConfigured && _isStarted;

        return result;
    }
}
