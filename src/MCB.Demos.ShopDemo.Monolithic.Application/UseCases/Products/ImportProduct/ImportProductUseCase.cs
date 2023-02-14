using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Application.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.RabbitMq.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.UnitOfWork.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Products.ImportProduct;
public class ImportProductUseCase
    : UseCaseBase<ImportProductUseCaseInput, Product>,
    IImportProductUseCase
{
    // Constants
    public const string IMPORT_PRODUCT_USE_CASE_TRACE_NAME = $"{nameof(ImportProductUseCase)}.{nameof(ExecuteInternalAsync)}";

    // Fields
    private readonly IProductService _productService;

    // Constructors
    public ImportProductUseCase(
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

    // Public Methods
    protected override Task<(bool Success, Product? Output)> ExecuteInternalAsync(ImportProductUseCaseInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: IMPORT_PRODUCT_USE_CASE_TRACE_NAME,
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
                        return q.Input.ProductService.ImportProductAsync(
                            input: q.Input.Adapter.Adapt<ImportProductUseCaseInput, ImportProductServiceInput>(q.Input.Input)!,
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