using MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Orders.Specifications;
public class OrderSpecifications
    : DomainEntitySpecifications,
    IOrderSpecifications
{
    // Constructors
    public OrderSpecifications(
        IDateTimeProvider dateTimeProvider
    ) : base(dateTimeProvider)
    {
    }

    // Methods
    public bool OrderShouldHaveCode(string code)
    {
        return !string.IsNullOrWhiteSpace(code);
    }
    public bool OrderShouldHaveDate(DateTime? date)
    {
        return date.HasValue;
    }
    public bool OrderShouldHaveDateWithValidLength(DateTime? date)
    {
        return date.HasValue && date <= DateTimeProvider.GetDate();
    }
    public bool OrderShouldHaveCustomer(Customer? customer)
    {
        return customer != null;
    }
    public bool OrderShouldHaveOrderItems(ImportOrderInput customer)
    {
        return customer.OrderItemCollection?.Any() == true;
    }
    public bool OrderShouldHaveOrderItemsWithValidSequence(ImportOrderInput customer)
    {
        var sequenceCollection = customer.OrderItemCollection.Select(q => q.Sequence).OrderBy(q => q);

        int? lastSequence = null;

        foreach (var sequence in sequenceCollection)
        {
            if (lastSequence is null)
            {
                if (sequence != 1)
                    return false;

                lastSequence = sequence;
            }
            else if (sequence - lastSequence != 1)
                return false;

            lastSequence = sequence;
        }

        return true;
    }
}
