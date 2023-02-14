using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.DeleteProduct;
public class DeleteProductUseCase
    : UseCaseBase<DeleteProductUseCaseInput, Product>,
    IDeleteProductUseCase
{
    // Constants
    public const string DELETE_PRODUCT_USE_CASE_TRACE_NAME = $"{nameof(DeleteProductUseCase)}.{nameof(ExecuteInternalAsync)}";

    // Fields
    private readonly IProductService _productService;

    // Constructors
    public DeleteProductUseCase(
        IDomainEventSubscriber domainEventSubscriber,
        INotificationPublisher notificationPublisher,
        IEventsExchangeRabbitMqPublisher eventsExchangeRabbitMqPublisher,
        IExternalEventFactory externalEventFactory,
        ITraceManager traceManager,
        IAdapter adapter,
        IUnitOfWork unitOfWork,
        IProductService productService
    ) : base(domainEventSubscriber, notificationPublisher, eventsExchangeRabbitMqPublisher, externalEventFactory, traceManager, adapter, unitOfWork)
    {
        _productService = productService;
    }

    protected override Task<(bool Success, Product? Output)> ExecuteInternalAsync(DeleteProductUseCaseInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: DELETE_PRODUCT_USE_CASE_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, UnitOfWork, ProductService: _productService, Adapter),
            handler: (input, activity, cancellationToken) =>
            {
                return input.UnitOfWork.ExecuteAsync(
                    handler: q =>
                    {
                        return q.Input.ProductService.DeleteProductAsync(
                            input: q.Input.Adapter.Adapt<DeleteProductUseCaseInput, DeleteProductServiceInput>(q.Input.Input)!,
                            cancellationToken
                        );
                    },
                    input: input,
                    openTransaction: false,
                    isBulkInsertOperation: false,
                    cancellationToken
                );
            },
            cancellationToken
        )!;
    }
}

