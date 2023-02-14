using Mapster;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Adapters;

public static class AdapterConfig
{
    // Public Methods
    public static void Configure()
    {
        TypeAdapterConfig<ImportCustomerServiceInput, ImportCustomerInput>.NewConfig();
        TypeAdapterConfig<ImportProductServiceInput, ImportProductInput>.NewConfig();

        TypeAdapterConfig<ValidationMessage, Notification>.NewConfig()
            .MapWith(
                converterFactory: src => new Notification(
                    (NotificationType)(int)src.ValidationMessageType,
                    src.Code,
                    src.Description
                ),
                applySettings: true
            );
    }
}
