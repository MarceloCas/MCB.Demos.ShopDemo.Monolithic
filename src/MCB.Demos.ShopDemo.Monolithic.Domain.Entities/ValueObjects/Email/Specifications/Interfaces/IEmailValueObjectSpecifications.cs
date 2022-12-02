using FluentValidation;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications.Interfaces;

public interface IEmailValueObjectSpecifications
{
    public static readonly string EmailValueObjectShouldRequiredErrorCode = nameof(EmailValueObjectShouldRequiredErrorCode);
    public static readonly string EmailValueObjectShouldRequiredMessage = nameof(EmailValueObjectShouldRequiredMessage);
    public static readonly Severity EmailValueObjectShouldRequiredSeverity = Severity.Error;

    public static readonly string EmailValueObjectShouldHaveMaximumLengthErrorCode = nameof(EmailValueObjectShouldHaveMaximumLengthErrorCode);
    public static readonly string EmailValueObjectShouldHaveMaximumLengthMessage = nameof(EmailValueObjectShouldHaveMaximumLengthMessage);
    public static readonly Severity EmailValueObjectShouldHaveMaximumLengthSeverity = Severity.Error;

    bool EmailValueObjectShouldRequired(EmailValueObject email);
    bool EmailValueObjectShouldHaveMaximumLength(EmailValueObject email);
}