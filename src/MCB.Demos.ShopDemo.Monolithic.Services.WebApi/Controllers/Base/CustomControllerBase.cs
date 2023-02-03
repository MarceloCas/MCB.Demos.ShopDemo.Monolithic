using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.FeatureFlag.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Enums;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;

public class CustomControllerBase
    : ControllerBase
{
    // Constants
    public const string METHOD_NOT_AVALIABLE_MESSAGE = "Method not avaliable. You can't have the feature flag {0}";

    // Fields
    private readonly INotificationSubscriber _notificationSubscriber;

    // Properties
    protected ILogger Logger { get; }
    protected ITraceManager TraceManager { get; }
    protected IAdapter Adapter { get; }
    protected IMcbFeatureFlagManager FeatureFlagManager { get; }

    // Constructors
    protected CustomControllerBase(
        ILogger logger,
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter,
        IMcbFeatureFlagManager featureFlagManager
    )
    {
        Logger = logger;
        _notificationSubscriber = notificationSubscriber;
        TraceManager = traceManager;
        Adapter = adapter;
        FeatureFlagManager = featureFlagManager;
    }

    // Private Methods
    private ResponseBase AddMessagesToResponse(ResponseBase response)
    {
        if (!_notificationSubscriber.NotificationCollection.Any())
            return response;

        response.Messages = _notificationSubscriber.NotificationCollection.Select(q =>
            new ResponseMessage
            {
                Type = q.NotificationType switch
                {
                    NotificationType.Information => ResponseMessageType.Information,
                    NotificationType.Warning => ResponseMessageType.Warning,
                    NotificationType.Error => ResponseMessageType.Error,
                    _ => throw new NotImplementedException(),
                },
                Code = q.Code,
                Description = q.Description,
                ResponseMessageCollection = q.NotificationCollection?.Select(r => new ResponseMessage
                {
                    Type = r.NotificationType switch
                    {
                        NotificationType.Information => ResponseMessageType.Information,
                        NotificationType.Warning => ResponseMessageType.Warning,
                        NotificationType.Error => ResponseMessageType.Error,
                        _ => throw new NotImplementedException(),
                    },
                    Code = r.Code,
                    Description = r.Description,
                })?.ToArray()
            }
        );

        return response;
    }
    private ResponseBase CreateResponse<TInput>(
        Func<TInput, ResponseBase> responseBaseFactory,
        TInput useCaseInput
    )
    {
        return AddMessagesToResponse(responseBaseFactory(useCaseInput));
    }

    // Protected Methods
    protected Task<bool> CheckFeatureFlagAsync(Guid tenantId, string? executionUser, string featureFlagKey, CancellationToken cancellationToken)
    {
        return FeatureFlagManager.GetFlagAsync(tenantId, executionUser, featureFlagKey, cancellationToken);
    }
    protected IActionResult CreateNotAllowedResult(string featureFlagKey)
    {
        return StatusCode(StatusCodes.Status503ServiceUnavailable, string.Format(METHOD_NOT_AVALIABLE_MESSAGE, featureFlagKey));
    }
    protected async Task<IActionResult> RunUseCaseAsync<TUseCaseInput, TUseCaseOutput>(
        IUseCase<TUseCaseInput, TUseCaseOutput> useCase,
        TUseCaseInput useCaseInput,
        Func<TUseCaseInput, ResponseBase> responseBaseFactory,
        int successStatusCode,
        int failStatusCode,
        CancellationToken cancellationToken
    ) where TUseCaseInput : UseCaseInputBase
    {
        var useCaseExecutionResult = await useCase.ExecuteAsync(
            useCaseInput,
            cancellationToken
        );

        var response =
            useCaseExecutionResult.Success ? null
            : CreateResponse(
                responseBaseFactory,
                useCaseInput
            );

        return StatusCode(
            statusCode: useCaseExecutionResult.Success ? successStatusCode : failStatusCode,
            value: response
        );
    }
    protected async Task<IActionResult> RunQueryAsync<TInput, TResult>(
        TInput input,
        Func<TInput, CancellationToken, Task<TResult?>> handler,
        Func<ResponseBase<TResult>> responseBaseFactory,
        CancellationToken cancellationToken
    )
    {
        var response = responseBaseFactory();
        var result = await handler(input, cancellationToken);

        response.Data = result;

        AddMessagesToResponse(response);

        return StatusCode(
            statusCode: response.Data != null ? StatusCodes.Status200OK : StatusCodes.Status404NotFound,
            value: response
        );
    }
}