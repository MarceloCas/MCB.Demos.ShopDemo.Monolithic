using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode.Interfaces;

namespace MCB.Demos.ShopDemo.Monolithic.Application.Queries.Products.GetProductByCode;
public class GetProductByCodeQuery
    : QueryBase<GetProductByCodeQueryInput, Product?>,
    IGetProductByCodeQuery
{
    // Constants
    public const string GET_PRODUCT_BY_CODE_QUERY_TRACE_NAME = $"{nameof(GetProductByCodeQuery)}.{nameof(ExecuteAsync)}";

    // Fields
    private readonly IProductRepository _productRepository;
    private readonly INotificationPublisher _notificationPublisher;

    // Constructors
    public GetProductByCodeQuery(
        ITraceManager traceManager,
        IProductRepository productRepository,
        INotificationPublisher notificationPublisher
    ) : base(traceManager)
    {
        _productRepository = productRepository;
        _notificationPublisher = notificationPublisher;
    }

    public override Task<Product?> ExecuteAsync(GetProductByCodeQueryInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: GET_PRODUCT_BY_CODE_QUERY_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            input.CorrelationId,
            input.TenantId,
            input.ExecutionUser,
            input.SourcePlatform,
            input,
            handler: (input, activity, cancellationToken) =>
            {
                return _productRepository.GetByCodeAsync(input!.TenantId, input.Code, cancellationToken);
            },
            cancellationToken
        );
    }
}
