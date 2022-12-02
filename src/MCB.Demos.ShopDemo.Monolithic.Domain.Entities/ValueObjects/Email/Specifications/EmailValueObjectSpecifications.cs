using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications;

public sealed class EmailValueObjectSpecifications
    : IEmailValueObjectSpecifications
{
    public bool EmailValueObjectShouldRequired(EmailValueObject email)
    {
        return !string.IsNullOrWhiteSpace(email.Address);
    }
    public bool EmailValueObjectShouldHaveMaximumLength(EmailValueObject email)
    {
        // A RFC 5321 (section 4.5.3)
        return email.Address.Length <= 256;
    }
}