using Mapster;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Payloads;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(TypeAdapterConfig typeAdapterConfig)
    {
        ConfigureForWebApi();

        Application.Adapters.AdapterConfig.Configure(typeAdapterConfig);
        Domain.Adapters.AdapterConfig.Configure(typeAdapterConfig);
        Infra.Data.Adapters.AdapterConfig.Configure(typeAdapterConfig);
    }

    // Private Methods
    private static void ConfigureForWebApi()
    {
        TypeAdapterConfig<RegisterNewCustomerPayload, RegisterNewCustomerUseCaseInput>.NewConfig();
    }
}