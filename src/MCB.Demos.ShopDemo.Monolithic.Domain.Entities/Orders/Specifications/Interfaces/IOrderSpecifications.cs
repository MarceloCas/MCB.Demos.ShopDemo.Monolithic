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
    public static readonly string OrderShouldHaveCodeMessage = nameof(OrderShouldHaveCodeMessage);
    public static readonly Severity OrderShouldHaveCodeSeverity = Severity.Error;

    // Date
    public static readonly string OrderShouldHaveDateErrorCode = nameof(OrderShouldHaveDateErrorCode);
    public static readonly string OrderShouldHaveDateMessage = nameof(OrderShouldHaveDateMessage);
    public static readonly Severity OrderShouldHaveDateSeverity = Severity.Error;

    public static readonly string OrderShouldHaveDateWithValidLengthErrorCode = nameof(OrderShouldHaveDateWithValidLengthErrorCode);
    public static readonly string OrderShouldHaveDateWithValidLengthMessage = nameof(OrderShouldHaveDateWithValidLengthMessage);
    public static readonly Severity OrderShouldHaveDateWithValidLengthSeverity = Severity.Error;

    // Customer
    public static readonly string OrderShouldHaveCustomerErrorCode = nameof(OrderShouldHaveCustomerErrorCode);
    public static readonly string OrderShouldHaveCustomerMessage = nameof(OrderShouldHaveCustomerMessage);
    public static readonly Severity OrderShouldHaveCustomerSeverity = Severity.Error;

    // OrderItems
    public static readonly string OrderShouldHaveOrderItemsErrorCode = nameof(OrderShouldHaveOrderItemsErrorCode);
    public static readonly string OrderShouldHaveOrderItemsMessage = nameof(OrderShouldHaveOrderItemsMessage);
    public static readonly Severity OrderShouldHaveOrderItemsSeverity = Severity.Error;

    public static readonly string OrderShouldHaveOrderItemsWithValidSequenceErrorCode = nameof(OrderShouldHaveOrderItemsWithValidSequenceErrorCode);
    public static readonly string OrderShouldHaveOrderItemsWithValidSequenceMessage = nameof(OrderShouldHaveOrderItemsWithValidSequenceMessage);
    public static readonly Severity OrderShouldHaveOrderItemsWithValidSequenceSeverity = Severity.Error;

    // Methods
    bool OrderShouldHaveCode(string? code);
    bool OrderShouldHaveDate(DateTime? date);
    bool OrderShouldHaveDateWithValidLength(DateTime? date);
    bool OrderShouldHaveCustomer(Customers.Customer? customer);
    bool OrderShouldHaveOrderItems(ImportOrderInput customer);
    bool OrderShouldHaveOrderItemsWithValidSequence(ImportOrderInput customer);
}
