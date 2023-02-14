using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Validators.Interfaces;
public interface IImportProductInputShouldBeValidValidator
    : IValidator<ImportProductInput>
{
}
