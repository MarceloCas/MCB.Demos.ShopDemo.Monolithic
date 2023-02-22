using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Validators.Interfaces;
public interface IImportOrderInputShouldBeValid
    : IValidator<ImportOrderInput>
{
}
