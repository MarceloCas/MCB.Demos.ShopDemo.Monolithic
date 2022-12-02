using Mapster;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Customers.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Adapters;

public class AdapterConfig
{
    // Public Methods
    public static void Configure(TypeAdapterConfig typeAdapterConfig)
    {
        typeAdapterConfig.ForType<RegisterNewCustomerServiceInput, RegisterNewCustomerInput>();
    }
}
