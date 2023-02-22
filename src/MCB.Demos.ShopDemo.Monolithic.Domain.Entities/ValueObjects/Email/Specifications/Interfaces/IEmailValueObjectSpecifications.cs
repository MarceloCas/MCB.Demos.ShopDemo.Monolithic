using FluentValidation;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications.Interfaces;

public interface IEmailValueObjectSpecifications
{
    // Constants
    public const int EMAIL_MAX_LENGTH = 256;

    public static readonly string EmailValueObjectShouldRequiredErrorCode = nameof(EmailValueObjectShouldRequiredErrorCode);
    public static readonly string EmailValueObjectShouldRequiredErrorMessage = nameof(EmailValueObjectShouldRequiredErrorMessage);
    public static readonly Severity EmailValueObjectShouldRequiredErrorSeverity = Severity.Error;

    public static readonly string EmailValueObjectShouldHaveMaximumLengthErrorCode = nameof(EmailValueObjectShouldHaveMaximumLengthErrorCode);
    public static readonly string EmailValueObjectShouldHaveMaximumLengthErrorMessage = nameof(EmailValueObjectShouldHaveMaximumLengthErrorMessage);
    public static readonly Severity EmailValueObjectShouldHaveMaximumLengthErrorSeverity = Severity.Error;

    bool EmailValueObjectShouldRequired(EmailValueObject email);
    bool EmailValueObjectShouldHaveMaximumLength(EmailValueObject email);
}