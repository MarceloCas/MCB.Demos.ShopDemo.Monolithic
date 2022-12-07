using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomerBatch.Interfaces;
public interface IRegisterNewCustomerBatchUseCase
    : IUseCase<RegisterNewCustomerBatchUseCaseInput>
{
}
