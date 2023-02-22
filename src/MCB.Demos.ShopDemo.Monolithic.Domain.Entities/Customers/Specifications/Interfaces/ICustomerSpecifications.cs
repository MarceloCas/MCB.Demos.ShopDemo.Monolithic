using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;

public interface ICustomerSpecifications
    : IDomainEntitySpecifications
{
    // Constants
    public const int CUSTOMER_FIRST_NAME_MAX_LENGTH = 150;
    public const int CUSTOMER_LAST_NAME_MAX_LENGTH = 250;
    public const int CUSTOMER_LEGAL_AGE = 18;

    // FirstName
    public static readonly string CustomerShouldHaveFirstNameErrorCode = nameof(CustomerShouldHaveFirstNameErrorCode);
    public static readonly string CustomerShouldHaveFirstNameErrorMessage = nameof(CustomerShouldHaveFirstNameErrorMessage);
    public static readonly Severity CustomerShouldHaveFirstNameErrorSeverity = Severity.Error;

    public static readonly string CustomerShouldHaveFirstNameMaximumLengthErrorCode = nameof(CustomerShouldHaveFirstNameMaximumLengthErrorCode);
    public static readonly string CustomerShouldHaveFirstNameMaximumLengthErrorMessage = nameof(CustomerShouldHaveFirstNameMaximumLengthErrorMessage);
    public static readonly Severity CustomerShouldHaveFirstNameMaximumLengthErrorSeverity = Severity.Error;

    // LastName
    public static readonly string CustomerShouldHaveLastNameErrorCode = nameof(CustomerShouldHaveLastNameErrorCode);
    public static readonly string CustomerShouldHaveLastNameErrorMessage = nameof(CustomerShouldHaveLastNameErrorMessage);
    public static readonly Severity CustomerShouldHaveLastNameErrorSeverity = Severity.Error;

    public static readonly string CustomerShouldHaveLastNameMaximumLengthErrorCode = nameof(CustomerShouldHaveLastNameMaximumLengthErrorCode);
    public static readonly string CustomerShouldHaveLastNameMaximumLengthErrorMessage = nameof(CustomerShouldHaveLastNameMaximumLengthErrorMessage);
    public static readonly Severity CustomerShouldHaveLastNameMaximumLengthErrorSeverity = Severity.Error;

    // BirthDate
    public static readonly string CustomerShouldHaveBirthDateErrorCode = nameof(CustomerShouldHaveBirthDateErrorCode);
    public static readonly string CustomerShouldHaveBirthDateErrorMessage = nameof(CustomerShouldHaveBirthDateErrorMessage);
    public static readonly Severity CustomerShouldHaveBirthDateErrorSeverity = Severity.Error;

    public static readonly string CustomerShouldHaveValidBirthDateErrorCode = nameof(CustomerShouldHaveValidBirthDateErrorCode);
    public static readonly string CustomerShouldHaveValidBirthDateErrorMessage = nameof(CustomerShouldHaveValidBirthDateErrorMessage);
    public static readonly Severity CustomerShouldHaveValidBirthDateErrorSeverity = Severity.Error;

    // Methods
    bool CustomerShouldHaveFirstName(string firstName);
    bool CustomerShouldHaveFirstNameMaximumLength(string firstName);

    bool CustomerShouldHaveLastName(string lastName);
    bool CustomerShouldHaveLastNameMaximumLength(string lastName);

    bool CustomerShouldHaveBirthDate(DateTime birthDate);
    bool CustomerShouldHaveValidBirthDate(DateTime birthDate);
}