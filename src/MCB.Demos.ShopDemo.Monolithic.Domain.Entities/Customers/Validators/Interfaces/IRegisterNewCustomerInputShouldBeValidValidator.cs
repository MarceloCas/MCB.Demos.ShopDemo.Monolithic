using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Validators.Interfaces;

public interface IRegisterNewCustomerInputShouldBeValidValidator
    : IValidator<RegisterNewCustomerInput>
{
}