using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.RegisterNewCustomer.Interfaces;

public interface IRegisterNewCustomerUseCase
    : IUseCase<RegisterNewCustomerUseCaseInput>
{
}