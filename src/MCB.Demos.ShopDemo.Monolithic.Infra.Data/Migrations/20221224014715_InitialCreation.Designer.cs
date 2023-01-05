﻿// <auto-generated />
using System;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Migrations
{
    [DbContext(typeof(DefaultEntityFrameworkDataContext))]
    [Migration("20221224014715_InitialCreation")]
    partial class InitialCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels.CustomerDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birthdate");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("createdby");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("lastname");

                    b.Property<string>("LastSourcePlatform")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("lastsourceplatform");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lastupdatedat");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("lastupdatedby");

                    b.Property<DateTime>("RegistryVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registryversion");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenantid");

                    b.HasKey("Id")
                        .HasName("PK_CUSTOMERS_CUSTOMER");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("IX_CREATED_AT");

                    b.HasIndex("CreatedBy")
                        .HasDatabaseName("IX_CREATED_BY");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("UK_EMAIL");

                    b.HasIndex("LastSourcePlatform")
                        .HasDatabaseName("IX_LAST_SOURCE_PLATFORM");

                    b.HasIndex("LastUpdatedAt")
                        .HasDatabaseName("IX_LAST_UPDATED_AT");

                    b.HasIndex("LastUpdatedBy")
                        .HasDatabaseName("IX_LAST_UPDATED_BY");

                    b.HasIndex("TenantId")
                        .HasDatabaseName("IX_TENANT_ID");

                    b.HasIndex("CreatedBy", "LastUpdatedBy")
                        .HasDatabaseName("IX_CREATED_BY_LAST_UPDATED_BY");

                    b.HasIndex("TenantId", "Id")
                        .HasDatabaseName("IX_TENANT_ID_ID");

                    b.ToTable("customer", "customers");
                });
#pragma warning restore 612, 618
        }
    }
}
