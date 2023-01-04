using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail.Interfaces;
public interface IGetCustomerByEmailQuery
    : IQuery<GetCustomerByEmailQueryInput, Customer?>
{
    public const NotificationType CUSTOMER_IS_UNDER_AGE_NOTIFICATION_TYPE = NotificationType.Warning;
    public const string CUSTOMER_IS_UNDER_AGE_MESSAGE_CODE = "CUSTOMER_IS_UNDER_AGE_MESSAGE_CODE";
    public const string CUSTOMER_IS_UNDER_AGE_MESSAGE_DESCRIPTION = "Customer is under age";
}
