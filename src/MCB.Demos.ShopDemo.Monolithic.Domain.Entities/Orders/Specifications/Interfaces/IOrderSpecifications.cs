using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;
public interface IOrderSpecifications
    : IDomainEntitySpecifications
{
    // Constants
    public const int ORDER_CODE_MAX_LENGTH = 250;

    // Code
    public static readonly string OrderShouldHaveCodeErrorCode = nameof(OrderShouldHaveCodeErrorCode);
    public static readonly string OrderShouldHaveCodeErrorMessage = nameof(OrderShouldHaveCodeErrorMessage);
    public static readonly Severity OrderShouldHaveCodeErrorSeverity = Severity.Error;

    // Date
    public static readonly string OrderShouldHaveDateErrorCode = nameof(OrderShouldHaveDateErrorCode);
    public static readonly string OrderShouldHaveDateErrorMessage = nameof(OrderShouldHaveDateErrorMessage);
    public static readonly Severity OrderShouldHaveDateErrorSeverity = Severity.Error;

    public static readonly string OrderShouldHaveDateWithValidLengthErrorCode = nameof(OrderShouldHaveDateWithValidLengthErrorCode);
    public static readonly string OrderShouldHaveDateWithValidLengthErrorMessage = nameof(OrderShouldHaveDateWithValidLengthErrorMessage);
    public static readonly Severity OrderShouldHaveDateWithValidLengthErrorSeverity = Severity.Error;

    // Customer
    public static readonly string OrderShouldHaveCustomerErrorCode = nameof(OrderShouldHaveCustomerErrorCode);
    public static readonly string OrderShouldHaveCustomerErrorMessage = nameof(OrderShouldHaveCustomerErrorMessage);
    public static readonly Severity OrderShouldHaveCustomerErrorSeverity = Severity.Error;

    // OrderItems
    public static readonly string OrderShouldHaveOrderItemsErrorCode = nameof(OrderShouldHaveOrderItemsErrorCode);
    public static readonly string OrderShouldHaveOrderItemsErrorMessage = nameof(OrderShouldHaveOrderItemsErrorMessage);
    public static readonly Severity OrderShouldHaveOrderItemsErrorSeverity = Severity.Error;

    public static readonly string OrderShouldHaveOrderItemsWithValidSequenceErrorCode = nameof(OrderShouldHaveOrderItemsWithValidSequenceErrorCode);
    public static readonly string OrderShouldHaveOrderItemsWithValidSequenceErrorMessage = nameof(OrderShouldHaveOrderItemsWithValidSequenceErrorMessage);
    public static readonly Severity OrderShouldHaveOrderItemsWithValidSequenceErrorSeverity = Severity.Error;

    // Methods
    bool OrderShouldHaveCode(string? code);
    bool OrderShouldHaveDate(DateTime? date);
    bool OrderShouldHaveDateWithValidLength(DateTime? date);
    bool OrderShouldHaveCustomer(Customers.Customer? customer);
    bool OrderShouldHaveOrderItems(ImportOrderInput customer);
    bool OrderShouldHaveOrderItemsWithValidSequence(ImportOrderInput customer);
}
