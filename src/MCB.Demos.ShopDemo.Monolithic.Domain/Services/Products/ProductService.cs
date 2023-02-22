using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductImported.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Events.ProductRemoved.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Services.Products;
public class ProductService
    : ServiceBase<Product>,
    IProductService
{
    // Constants
    public const string IMPORT_PRODUCT_TRACE_NAME = $"{nameof(ProductService)}.{nameof(ImportProductAsync)}";
    public const string VALIDATE_IMPORT_PRODUCT_TRACE_NAME = $"{nameof(ProductService)}.{nameof(ValidateImportProductAsync)}";
    public const string DELETE_PRODUCT_TRACE_NAME = $"{nameof(ProductService)}.{nameof(RemoveProductAsync)}";

    // Messages
    public static readonly string ProductCodeAlreadyRegisteredErrorCode = nameof(ProductCodeAlreadyRegisteredErrorCode);
    public static readonly string ProductCodeAlreadyRegisteredMessage = nameof(ProductCodeAlreadyRegisteredMessage);
    public static readonly NotificationType ProductCodeAlreadyRegisteredNotificationType = NotificationType.Error;

    public static readonly string ProductNotFoundErrorCode = nameof(ProductNotFoundErrorCode);
    public static readonly string ProductNotFoundMessage = nameof(ProductNotFoundMessage);
    public static readonly NotificationType ProductNotFoundNotificationType = NotificationType.Error;

    // Fields
    private readonly IProductRepository _productRepository;
    private readonly IProductFactory _productFactory;
    private readonly IProductImportedDomainEventFactory _productHasBeenRegisteredDomainEventFactory;
    private readonly IProductRemovedDomainEventFactory _productRemovedDomainEventFactory;

    // Constructors
    public ProductService(
        INotificationPublisher notificationPublisher,
        IDomainEventPublisher domainEventPublisher,
        ITraceManager traceManager,
        IAdapter adapter,
        IProductRepository productRepository,
        IProductFactory productFactory,
        IProductImportedDomainEventFactory productHasBeenRegisteredDomainEventFactory,
        IProductRemovedDomainEventFactory productRemovedDomainEventFactory
    ) : base(notificationPublisher, domainEventPublisher, traceManager, adapter, productRepository)
    {
        _productRepository = productRepository;
        _productFactory = productFactory;
        _productHasBeenRegisteredDomainEventFactory = productHasBeenRegisteredDomainEventFactory;
        _productRemovedDomainEventFactory = productRemovedDomainEventFactory;
    }

    // Public Methods
    public Task<(bool Success, Product? ImportedProduct)> ImportProductAsync(ImportProductServiceInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: IMPORT_PRODUCT_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, ProductRepository: _productRepository, Adapter, NotificationPublisher, ProductFactory: _productFactory, DomainEventPublisher, ProductHasBeenRegisteredDomainEventFactory: _productHasBeenRegisteredDomainEventFactory),
            handler: async (input, activity, cancellationToken) =>
            {
                // Validate input before process
                if (await input.ProductRepository.GetByCodeAsync(input.Input.TenantId, input.Input.Code, cancellationToken) is not null)
                {
                    await input.NotificationPublisher.PublishNotificationAsync(
                        new Notification(
                            ProductCodeAlreadyRegisteredNotificationType,
                            ProductCodeAlreadyRegisteredErrorCode,
                            ProductCodeAlreadyRegisteredMessage
                        ),
                        cancellationToken
                    );

                    return default;
                }

                // Process
                var product = input.ProductFactory
                    .Create()!
                    .ImportProduct(input.Adapter.Adapt<ImportProductServiceInput, ImportProductInput>(input.Input)!);

                // Validate domain entity after process
                if (!await ValidateDomainEntityAndSendNotificationsAsync(product, cancellationToken))
                    return default;

                // Persist
                var persistenceResult = await input.ProductRepository.ImportProductAsync(product, cancellationToken);
                if (!persistenceResult.Success)
                    return default;

                // Send domain event
                await input.DomainEventPublisher.PublishDomainEventAsync(
                    input.ProductHasBeenRegisteredDomainEventFactory.Create((product, input.Input.ExecutionUser, input.Input.SourcePlatform, input.Input.CorrelationId))!,
                    cancellationToken
                );

                // Return
                return (Success: true, ImportedProduct: product);
            },
            cancellationToken
        )!;
    }
    public Task<(bool Success, IEnumerable<Notification>? NotificationCollection)> ValidateImportProductAsync(ValidateImportProductServiceInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: VALIDATE_IMPORT_PRODUCT_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, ProductRepository: _productRepository, Adapter, NotificationPublisher, ProductFactory: _productFactory, DomainEventPublisher, ProductHasBeenRegisteredDomainEventFactory: _productHasBeenRegisteredDomainEventFactory),
            handler: async (input, activity, cancellationToken) =>
            {
                var notificationCollection = new List<Notification>();

                // Validate input before process
                if (await input.ProductRepository.GetByCodeAsync(input.Input.TenantId, input.Input.Code, cancellationToken) is not null)
                    notificationCollection.Add(
                    new Notification(
                            ProductCodeAlreadyRegisteredNotificationType,
                            ProductCodeAlreadyRegisteredErrorCode,
                            ProductCodeAlreadyRegisteredMessage
                        )
                    );

                // Process
                var product = input.ProductFactory
                    .Create()!
                    .ImportProduct(input.Adapter.Adapt<ValidateImportProductServiceInput, ImportProductInput>(input.Input)!);

                // Validate domain entity after process
                foreach (var validationMessage in product.ValidationInfo.ValidationMessageCollection)
                    notificationCollection.Add(Adapter.Adapt<ValidationMessage, Notification>(validationMessage));

                // Return
                return (Success: notificationCollection.Count == 0, NotificationCollection: notificationCollection.Count == 0 ? null : notificationCollection.AsEnumerable());
            },
            cancellationToken
        )!;
    }
    public Task<(bool Success, Product? RemovedProduct)> RemoveProductAsync(RemoveProductServiceInput input, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: DELETE_PRODUCT_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: input.CorrelationId,
            tenantId: input.TenantId,
            executionUser: input.ExecutionUser,
            sourcePlatform: input.SourcePlatform,
            input: (Input: input, ProductRepository: _productRepository, Adapter, NotificationPublisher, ProductFactory: _productFactory, DomainEventPublisher, ProductRemovedDomainEventFactory: _productRemovedDomainEventFactory),
            handler: async (input, activity, cancellationToken) =>
            {
                // Validate input before process
                var product = await input.ProductRepository.GetByCodeAsync(input.Input.TenantId, input.Input.Code, cancellationToken);

                if (product is null)
                {
                    await input.NotificationPublisher.PublishNotificationAsync(
                        new Notification(
                            ProductNotFoundNotificationType,
                            ProductNotFoundErrorCode,
                            ProductNotFoundMessage
                        ),
                        cancellationToken
                    );

                    return default;
                }

                // Process
                var persistenceResult = await input.ProductRepository.RemoveProductAsync(product, cancellationToken);
                if (!persistenceResult)
                    return default;

                // Send domain event
                await input.DomainEventPublisher.PublishDomainEventAsync(
                    input.ProductRemovedDomainEventFactory.Create((product, input.Input.ExecutionUser, input.Input.SourcePlatform, input.Input.CorrelationId))!,
                    cancellationToken
                );

                // Return
                return (Success: persistenceResult, RemovedProduct: product);
            },
            cancellationToken
        )!;
    }
}
