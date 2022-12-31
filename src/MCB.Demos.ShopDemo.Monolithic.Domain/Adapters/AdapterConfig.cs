using Mapster;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(TypeAdapterConfig typeAdapterConfig)
    {
        typeAdapterConfig.ForType<RegisterNewCustomerServiceInput, RegisterNewCustomerInput>();
        typeAdapterConfig.ForType<ValidationMessage, Notification>()
            .MapWith(
                converterFactory: src => new Notification(
                    (NotificationType)(int)src.ValidationMessageType,
                    src.Code,
                    src.Description,
                    Enumerable.Empty<Notification>()
                ),
                applySettings: true
            );
    }
}
