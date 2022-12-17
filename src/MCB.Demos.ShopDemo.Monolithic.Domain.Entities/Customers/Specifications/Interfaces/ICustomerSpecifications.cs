using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;

public interface ICustomerSpecifications
    : IDomainEntitySpecifications
{
    // Constants
    public const int CUSTOMER_FIRST_NAME_MAX_LENGTH = 150;
    public const int CUSTOMER_LAST_NAME_MAX_LENGTH = 250;

    // FirstName
    public static readonly string CustomerShouldHaveFirstNameErrorCode = nameof(CustomerShouldHaveFirstNameErrorCode);
    public static readonly string CustomerShouldHaveFirstNameMessage = nameof(CustomerShouldHaveFirstNameMessage);
    public static readonly Severity CustomerShouldHaveFirstNameSeverity = Severity.Error;

    public static readonly string CustomerShouldHaveFirstNameMaximumLengthErrorCode = nameof(CustomerShouldHaveFirstNameMaximumLengthErrorCode);
    public static readonly string CustomerShouldHaveFirstNameMaximumLengthMessage = nameof(CustomerShouldHaveFirstNameMaximumLengthMessage);
    public static readonly Severity CustomerShouldHaveFirstNameMaximumLengthSeverity = Severity.Error;

    // LastName
    public static readonly string CustomerShouldHaveLastNameErrorCode = nameof(CustomerShouldHaveLastNameErrorCode);
    public static readonly string CustomerShouldHaveLastNameMessage = nameof(CustomerShouldHaveLastNameMessage);
    public static readonly Severity CustomerShouldHaveLastNameSeverity = Severity.Error;

    public static readonly string CustomerShouldHaveLastNameMaximumLengthErrorCode = nameof(CustomerShouldHaveLastNameMaximumLengthErrorCode);
    public static readonly string CustomerShouldHaveLastNameMaximumLengthMessage = nameof(CustomerShouldHaveLastNameMaximumLengthMessage);
    public static readonly Severity CustomerShouldHaveLastNameMaximumLengthSeverity = Severity.Error;

    // BirthDate
    public static readonly string CustomerShouldHaveBirthDateErrorCode = nameof(CustomerShouldHaveBirthDateErrorCode);
    public static readonly string CustomerShouldHaveBirthDateMessage = nameof(CustomerShouldHaveBirthDateMessage);
    public static readonly Severity CustomerShouldHaveBirthDateSeverity = Severity.Error;

    public static readonly string CustomerShouldHaveValidBirthDateErrorCode = nameof(CustomerShouldHaveValidBirthDateErrorCode);
    public static readonly string CustomerShouldHaveValidBirthDateMessage = nameof(CustomerShouldHaveValidBirthDateMessage);
    public static readonly Severity CustomerShouldHaveValidBirthDateSeverity = Severity.Error;

    // Methods
    bool CustomerShouldHaveFirstName(string firstName);
    bool CustomerShouldHaveFirstNameMaximumLength(string firstName);

    bool CustomerShouldHaveLastName(string lastName);
    bool CustomerShouldHaveLastNameMaximumLength(string lastName);

    bool CustomerShouldHaveBirthDate(DateTime birthDate);
    bool CustomerShouldHaveValidBirthDate(DateTime birthDate);
}