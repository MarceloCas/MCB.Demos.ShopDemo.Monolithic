using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Products.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations;
public class ProductEntityTypeConfiguration
    : EntityTypeConfigurationBase<ProductDataModel>
{
    // Constructors
    public ProductEntityTypeConfiguration()
        : base(schemaName: "products", "product")
    {
    }

    // Protected Methods
    protected override void ConfigureInternal(EntityTypeBuilder<ProductDataModel> builder)
    {
        // Code
        builder.Property(q => q.Code)
            .HasColumnName(nameof(ProductDataModel.Code).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(IProductSpecifications.PRODUCT_CODE_MAX_LENGTH);
        builder.HasIndex(q => new { q.TenantId, q.Code })
            .IsUnique()
            .HasDatabaseName("UK_CODE");

        // Description
        builder.Property(q => q.Description)
            .HasColumnName(nameof(ProductDataModel.Description).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(IProductSpecifications.PRODUCT_DESCRIPTION_MAX_LENGTH);
    }
}
