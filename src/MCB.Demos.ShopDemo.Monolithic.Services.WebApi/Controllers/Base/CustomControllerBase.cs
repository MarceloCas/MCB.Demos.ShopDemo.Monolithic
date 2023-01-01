using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Core.Infra.CrossCutting.Observability.OpenTelemetry;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Enums;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Base;

public class CustomControllerBase
    : ControllerBase
{
    // Fields
    private readonly INotificationSubscriber _notificationSubscriber;

    // Properties
    protected ITraceManager TraceManager { get; }
    protected IAdapter Adapter { get; }

    // Constructors
    protected CustomControllerBase(
        INotificationSubscriber notificationSubscriber,
        ITraceManager traceManager,
        IAdapter adapter
    )
    {
        _notificationSubscriber = notificationSubscriber;
        TraceManager = traceManager;
        Adapter = adapter;
    }

    // Private Methods
    private ResponseBase CreateResponse<TUseCaseInput>(
        Func<TUseCaseInput, ResponseBase> responseBaseFactory,
        TUseCaseInput useCaseInput
    )
    {
        var response = responseBaseFactory(useCaseInput);

        response.Messages = _notificationSubscriber.NotificationCollection.Select(q =>
            new ResponseMessage{
                Type = q.NotificationType switch
                {
                    NotificationType.Information => ResponseMessageType.Information,
                    NotificationType.Warning => ResponseMessageType.Warning,
                    NotificationType.Error => ResponseMessageType.Error,
                    _ => throw new NotImplementedException(),
                },
                Code = q.Code,
                Description = q.Description,
                ResponseMessageCollection = q.NotificationCollection.Select(r => new ResponseMessage
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
                }).ToArray()
            }
        );

        return response;
    }

    // Protected Methods
    protected async Task<IActionResult> RunUseCaseAsync<TUseCaseInput>(
        IUseCase<TUseCaseInput> useCase,
        TUseCaseInput useCaseInput,
        Func<TUseCaseInput, ResponseBase> responseBaseFactory,
        int successStatusCode,
        int failStatusCode,
        CancellationToken cancellationToken
    ) where TUseCaseInput : UseCaseInputBase
    {
        var success = await useCase.ExecuteAsync(
            useCaseInput,
            cancellationToken
        );

        var response =
            success ? null
            : CreateResponse(
                responseBaseFactory,
                useCaseInput
            );

        return StatusCode(
            statusCode: success ? successStatusCode : failStatusCode,
            value: response
        );
    }
}