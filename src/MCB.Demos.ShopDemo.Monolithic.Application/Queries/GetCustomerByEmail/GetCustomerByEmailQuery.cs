using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.GetCustomerByEmail;

public class GetCustomerByEmailQuery
    : QueryBase<GetCustomerByEmailQueryInput, Customer?>,
    IGetCustomerByEmailQuery
{
    // Fields
    private readonly ICustomerRepository _customerRepository;
    private readonly INotificationPublisher _notificationPublisher;

    // Constructors
    public GetCustomerByEmailQuery(
        ITraceManager traceManager,
        ICustomerRepository customerRepository,
        INotificationPublisher notificationPublisher
    ) : base(traceManager)
    {
        _customerRepository = customerRepository;
        _notificationPublisher = notificationPublisher;
    }

    public override Task<Customer?> ExecuteAsync(GetCustomerByEmailQueryInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: $"{nameof(GetCustomerByEmailQuery)}.{nameof(ExecuteAsync)}",
            kind: System.Diagnostics.ActivityKind.Internal,
            input.CorrelationId,
            input.TenantId,
            input.ExecutionUser,
            input.SourcePlatform,
            input,
            handler: async (input, activity, cancellationToken) =>
            {
                var customer = await _customerRepository.GetByEmailAsync(input!.TenantId, input.Email, cancellationToken);

                if (customer?.Age < ICustomerSpecifications.CUSTOMER_LEGAL_AGE)
                    await _notificationPublisher.PublishNotificationAsync(
                        new Notification(
                            notificationType: IGetCustomerByEmailQuery.CUSTOMER_IS_UNDER_AGE_NOTIFICATION_TYPE,
                            code: IGetCustomerByEmailQuery.CUSTOMER_IS_UNDER_AGE_MESSAGE_CODE,
                            description: IGetCustomerByEmailQuery.CUSTOMER_IS_UNDER_AGE_MESSAGE_DESCRIPTION
                        ),
                        cancellationToken
                    );

                return customer;
            },
            cancellationToken
        );
    }
}
