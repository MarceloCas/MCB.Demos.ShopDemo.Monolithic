using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Customers.Specifications.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Domain.Entities.ValueObjects.Email.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.EntityTypeConfigurations;

public class CustomerEntityTypeConfiguration
    : EntityTypeConfigurationBase<CustomerDataModel>
{
    // Constructors
    public CustomerEntityTypeConfiguration ()
        : base(schemaName: "customers", "customer")
    {
    }

    // Protected Methods
    protected override void ConfigureInternal(EntityTypeBuilder<CustomerDataModel> builder)
    {
        // First Name
        builder.Property(q => q.FirstName)
            .HasColumnName(nameof(CustomerDataModel.FirstName).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(ICustomerSpecifications.CUSTOMER_FIRST_NAME_MAX_LENGTH);

        // Last Name
        builder.Property(q => q.LastName)
            .HasColumnName(nameof(CustomerDataModel.LastName).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(ICustomerSpecifications.CUSTOMER_LAST_NAME_MAX_LENGTH);

        // Birth Date
        builder.Property(q => q.BirthDate)
            .HasColumnName(nameof(CustomerDataModel.BirthDate).ToLowerInvariant())
            .IsRequired();

        // Email
        builder.Property(q => q.Email)
            .HasColumnName(nameof(CustomerDataModel.Email).ToLowerInvariant())
            .IsRequired()
            .HasMaxLength(IEmailValueObjectSpecifications.EMAIL_MAX_LENGTH);
        builder.HasIndex(q => q.Email)
            .IsUnique()
            .HasDatabaseName("UK_EMAIL");
    }
}
