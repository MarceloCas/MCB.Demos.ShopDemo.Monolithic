using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
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
    protected IAdapter Adapter { get; }

    // Constructors
    protected CustomControllerBase(
        INotificationSubscriber notificationSubscriber,
        IAdapter adapter
    )
    {
        _notificationSubscriber = notificationSubscriber;
        Adapter = adapter;
    }

    // Private Methods
    private ResponseBase CreateResponse<TUseCaseInput>(
        string? executionUser,
        string? sourcePlatform,
        Func<TUseCaseInput, ResponseBase> responseBaseFactory,
        TUseCaseInput useCaseInput
    )
    {
        var response = responseBaseFactory(useCaseInput);

        response.ExecutionUser = executionUser;
        response.SourcePlatform = sourcePlatform;

        response.ResponseMessageCollection = _notificationSubscriber.NotificationCollection.Select(q =>
            new ResponseMessage(
                type: q.NotificationType switch
                {
                    NotificationType.Information => ResponseMessageType.Information,
                    NotificationType.Warning => ResponseMessageType.Warning,
                    NotificationType.Error => ResponseMessageType.Error,
                    _ => throw new NotImplementedException(),
                },
                code: q.Code,
                description: q.Description
            )
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

        return StatusCode(
            statusCode: success ? successStatusCode : failStatusCode,
            value: CreateResponse(
                useCaseInput.ExecutionUser,
                useCaseInput.SourcePlatform,
                responseBaseFactory,
                useCaseInput
            )
        );
    }
}