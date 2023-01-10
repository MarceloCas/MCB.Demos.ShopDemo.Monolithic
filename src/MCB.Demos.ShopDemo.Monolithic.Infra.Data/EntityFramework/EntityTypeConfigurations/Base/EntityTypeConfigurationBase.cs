using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations.Base;
public abstract class EntityTypeConfigurationBase<TDataModelBase>
    : IEntityTypeConfiguration<TDataModelBase>
    where TDataModelBase : DataModelBase
{
    // Fields
    private readonly string _schemaName;
    private readonly string _tableName;

    // Constructors
    protected EntityTypeConfigurationBase(string schemaName, string tableName)
    {
        _schemaName = schemaName;
        _tableName = tableName;
    }

    // Public Methods
    public void Configure(EntityTypeBuilder<TDataModelBase> builder)
    {
        // Table config
        builder.ToTable(name: _tableName, schema: _schemaName);

        // Id
        builder.Property(q => q.Id)
            .HasColumnName(nameof(DataModelBase.Id).ToLowerInvariant())
            .IsRequired()
            .ValueGeneratedNever();
        builder.HasKey(q => q.Id)
            .HasName($"PK_{_schemaName.ToUpperInvariant()}_{_tableName.ToUpperInvariant()}");

        // TenantId
        builder.Property(q => q.TenantId)
            .HasColumnName(nameof(DataModelBase.TenantId).ToLowerInvariant())
            .IsRequired();
        builder.HasIndex(q => q.TenantId)
            .HasDatabaseName("IX_TENANT_ID");

        builder.HasIndex(q => new { q.TenantId, q.Id })
            .HasDatabaseName("IX_TENANT_ID_ID");

        // CreatedBy
        builder.Property(q => q.CreatedBy)
            .HasColumnName(nameof(DataModelBase.CreatedBy).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(250);
        builder.HasIndex(q => new { q.TenantId, q.CreatedBy })
            .HasDatabaseName("IX_CREATED_BY");

        // CreatedAt
        builder.Property(q => q.CreatedAt)
            .HasColumnName(nameof(DataModelBase.CreatedAt).ToLowerInvariant())
            .IsRequired();
        builder.HasIndex(q => new { q.TenantId, q.CreatedAt })
            .HasDatabaseName("IX_CREATED_AT");

        // LastUpdatedBy
        builder.Property(q => q.LastUpdatedBy)
            .HasColumnName(nameof(DataModelBase.LastUpdatedBy).ToLowerInvariant())
            .IsRequired(false)
            .HasMaxLength(250);
        builder.HasIndex(q => new { q.TenantId, q.LastUpdatedBy })
            .HasDatabaseName("IX_LAST_UPDATED_BY");

        // LastUpdatedAt
        builder.Property(q => q.LastUpdatedAt)
            .HasColumnName(nameof(DataModelBase.LastUpdatedAt).ToLowerInvariant())
            .IsRequired(false);
        builder.HasIndex(q => new { q.TenantId, q.LastUpdatedAt })
            .HasDatabaseName("IX_LAST_UPDATED_AT");

        builder.HasIndex(q => new { q.TenantId, q.CreatedBy, q.LastUpdatedBy })
            .HasDatabaseName("IX_CREATED_BY_LAST_UPDATED_BY");

        // LastSourcePlatform
        builder.Property(q => q.LastSourcePlatform)
            .HasColumnName(nameof(DataModelBase.LastSourcePlatform).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(250);
        builder.HasIndex(q => new { q.TenantId, q.LastSourcePlatform })
            .HasDatabaseName("IX_LAST_SOURCE_PLATFORM");

        // LastCorrelationId
        builder.Property(q => q.LastCorrelationId)
            .HasColumnName(nameof(DataModelBase.LastCorrelationId).ToLowerInvariant())
            .IsRequired();
        builder.HasIndex(q => new { q.TenantId, q.LastCorrelationId })
            .HasDatabaseName("IX_LAST_CORRELATION_ID");

        // RegistryVersion
        builder.Property(q => q.RegistryVersion)
            .HasColumnName(nameof(DataModelBase.RegistryVersion).ToLowerInvariant())
            .IsRequired()
            .IsConcurrencyToken();

        ConfigureInternal(builder);
    }

    // Protected Methods
    protected abstract void ConfigureInternal(EntityTypeBuilder<TDataModelBase> builder);
}
