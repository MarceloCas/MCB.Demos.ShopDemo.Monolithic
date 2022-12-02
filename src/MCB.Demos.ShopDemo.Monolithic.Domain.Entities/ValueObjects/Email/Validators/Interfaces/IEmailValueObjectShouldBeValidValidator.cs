using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Validators.Interfaces;

public interface IEmailValueObjectShouldBeValidValidator
    : IValidator<EmailValueObject>
{
}