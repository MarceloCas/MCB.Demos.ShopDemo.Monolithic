using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Domain.Repositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Redis.DataModelsRepositories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories.Base;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Factories.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Inputs;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Repositories;
public class ProductRepository
    : RepositoryBase,
    IProductRepository
{
    // Constants
    public const string GET_BY_CODE_TRACE_NAME = $"{nameof(ProductRepository)}.{nameof(GetByCodeAsync)}";
    public const string IMPORT_PRODUCT_TRACE_NAME = $"{nameof(ProductRepository)}.{nameof(ImportProductAsync)}";
    public const string DELETE_PRODUCT_TRACE_NAME = $"{nameof(ProductRepository)}.{nameof(DeleteProductAsync)}";

    // Fields
    private readonly TimeSpan _productDataModelTTL;
    private readonly IProductFactory _productFactory;
    private readonly IProductDataModelEntityFrameworkRepository _productDataModelRepository;
    private readonly IProductDataModelRedisRepository _productDataModelRedisRepository;

    // Constructors
    public ProductRepository(
        ITraceManager traceManager,
        IAdapter adapter,
        AppSettings appSettings,
        IProductFactory productFactory,
        IProductDataModelEntityFrameworkRepository productDataModelRepository,
        IProductDataModelRedisRepository productDataModelRedisRepository
    ) : base(traceManager, adapter)
    {
        _productDataModelTTL = TimeSpan.FromSeconds(appSettings.Redis.TtlSeconds.ProductDataModel);
        _productFactory = productFactory;
        _productDataModelRepository = productDataModelRepository;
        _productDataModelRedisRepository = productDataModelRedisRepository;
    }

    // Public Methods
    public Task<Product?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: GET_BY_CODE_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: tenantId,
            executionUser: string.Empty,
            sourcePlatform: string.Empty,
            input: (
                TenantId: tenantId,
                Code: code,
                ProductDataModelRepository: _productDataModelRepository,
                ProductDataModelRedisRepository: _productDataModelRedisRepository,
                ProductFactory: _productFactory,
                Adapter
            ),
            handler: async (input, activity, cancellationToken) =>
            {
                var productDataModel = await input.ProductDataModelRedisRepository.GetAsync(
                    input.ProductDataModelRedisRepository.GetKey(input.TenantId, input.Code)
                );

                if (productDataModel != null)
                    return input.ProductFactory.Create()!.SetExistingProductInfo(
                        input.Adapter.Adapt<SetExistingProductInfoInput>(productDataModel)!
                    );

                productDataModel = await input.ProductDataModelRepository.GetByCodeAsync(
                    input.TenantId,
                    input.Code,
                    cancellationToken
                );

                if (productDataModel is null)
                    return null;

                await input.ProductDataModelRedisRepository.AddOrUpdateAsync(
                    productDataModel,
                    expiry: _productDataModelTTL,
                    cancellationToken
                );

                return input.ProductFactory.Create()!.SetExistingProductInfo(
                    input.Adapter.Adapt<SetExistingProductInfoInput>(productDataModel)!
                );
            },
            cancellationToken
        )!;
    }

    public Task<(bool Success, int ModifiedCount)> ImportProductAsync(Product product, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: IMPORT_PRODUCT_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: product.TenantId,
            executionUser: product.AuditableInfo.CreatedBy,
            sourcePlatform: product.AuditableInfo.LastSourcePlatform,
            input: (Product: product, Adapter, ProductDataModelRepository: _productDataModelRepository),
            handler: async (input, activity, cancellationToken) =>
            {
                var productDataModel = input.Adapter.Adapt<Product, ProductDataModel>(input.Product);

                if (productDataModel is null)
                    return default;

                await input.ProductDataModelRepository.AddAsync(productDataModel, cancellationToken);

                return (Success: true, ModifiedCount: 1);
            },
            cancellationToken
        )!;
    }
    public Task<bool> DeleteProductAsync(Product product, CancellationToken cancellationToken)
    {
        return TraceManager.StartActivityAsync(
            name: DELETE_PRODUCT_TRACE_NAME,
            kind: System.Diagnostics.ActivityKind.Internal,
            correlationId: Guid.Empty,
            tenantId: product.TenantId,
            executionUser: product.AuditableInfo.CreatedBy,
            sourcePlatform: product.AuditableInfo.LastSourcePlatform,
            input: (Product: product, Adapter, ProductDataModelRepository: _productDataModelRepository, ProductDataModelRedisRepository: _productDataModelRedisRepository),
            handler: async (input, activity, cancellationToken) =>
            {
                var productDataModel = input.Adapter.Adapt<Product, ProductDataModel>(input.Product);

                if (productDataModel is null)
                    return false;

                await input.ProductDataModelRepository.RemoveAsync(productDataModel, cancellationToken);
                await input.ProductDataModelRedisRepository.RemoveAsync(productDataModel, cancellationToken);

                return true;
            },
            cancellationToken
        )!;
    }
}
