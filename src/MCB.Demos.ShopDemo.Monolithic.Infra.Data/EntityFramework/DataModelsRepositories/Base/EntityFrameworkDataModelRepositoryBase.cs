﻿using MCB.Core.Infra.CrossCutting.Observability.Abstractions;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataContexts.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base.Interfaces;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Models;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.ResiliencePolicies.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using NpgsqlTypes;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base;
public abstract class EntityFrameworkDataModelRepositoryBase
{
    // Constants
    public const string MAPPED_PROPERTY_METADADA_COLLECTION_CANNOT_BE_NULL = $"MappedPropertyMetadadaCollection cannot be null";
    public const string DATA_MODEL_MUST_BE_INITIALIZED = $"DataModel must be initialized";
    public const string BINARY_IMPORT_COMMAND_TEMPLATE = "COPY {0} ({1}) FROM STDIN (FORMAT BINARY)";

    // Properties
    protected static Dictionary<Type, MappedMetadata> MappedMetadataDictionary { get; } = new();

    // Protected Methods
    protected static NpgsqlDbType GetNpgsqlDbType(Type type)
    {
        var returnType = NpgsqlDbType.Unknown;

        if (type == typeof(Guid))
            returnType = NpgsqlDbType.Uuid;
        else if (type == typeof(string))
            returnType = NpgsqlDbType.Varchar;
        else if (type == typeof(DateTime))
            returnType = NpgsqlDbType.TimestampTz;

        return returnType;
    }
    protected static void Initialize(
        Type dataModelType,
        string? schemaName, 
        string tableName,
        MappedPropertyMetadada[] mappedPropertyMetadadaCollection
    )
    {
        if (mappedPropertyMetadadaCollection is null)
            throw new InvalidOperationException(MAPPED_PROPERTY_METADADA_COLLECTION_CANNOT_BE_NULL);

        if (MappedMetadataDictionary.ContainsKey(dataModelType))
            return;

        MappedMetadataDictionary.Add(
            dataModelType,
            new MappedMetadata(
                SchemaName: schemaName,
                TableName: tableName,
                MappedPropertyMetadadaCollection: mappedPropertyMetadadaCollection,
                BinaryImportCommand: string.Format(
                    BINARY_IMPORT_COMMAND_TEMPLATE,
                    $"{(schemaName is null ? string.Empty : $"{schemaName}.")}{tableName}", //Destiny
                    string.Join(',', mappedPropertyMetadadaCollection.Select(q => q.ColumnName)) // Columns
                )
            )
        );
    }
}

public abstract class EntityFrameworkDataModelRepositoryBase<TDataModel>
    : EntityFrameworkDataModelRepositoryBase,
    IEntityFrameworkDataModelRepository<TDataModel>
    where TDataModel : DataModelBase
{
    // Constants
    public const string ENTITY_TYPE_NOT_FOUND_MESSAGE = "Entity Type not found [{0}]";
    public const string CONNECTION_MUST_BE_NPGSQLCONNECTION_MESSAGE = "Connection must be a NpgsqlConnection";
    public const string NPGSQL_BINARY_IMPORTER_CANNOT_BE_NULL = "NpgsqlBinaryImporter cannot be null";

    // Fields
    private readonly bool _hasInitialized;
    private MappedMetadata? _currentMappedMetadata;
    private bool _hasBulkedData;

    // Properties
    protected IEntityFrameworkDataContext EntityFrameworkDataContext { get; }
    protected ITraceManager TraceManager { get; }
    protected DbSet<TDataModel> DbSet { get; }
    protected IPostgreSqlResiliencePolicy PostgreSqlResiliencePolicy { get; }
    protected NpgsqlBinaryImporter? CurrentNpgsqlBinaryImporter { get; private set; }

    // Constructors
    protected EntityFrameworkDataModelRepositoryBase(
        IEntityFrameworkDataContext entityFrameworkDataContext,
        ITraceManager traceManager,
        IPostgreSqlResiliencePolicy postgreSqlResiliencePolicy
    )
    {
        EntityFrameworkDataContext = entityFrameworkDataContext;
        TraceManager = traceManager;
        DbSet = entityFrameworkDataContext.GetDbSet<TDataModel>();
        PostgreSqlResiliencePolicy = postgreSqlResiliencePolicy;

        if (!_hasInitialized)
        {
            EntityFrameworkDataModelRepositoryBase<TDataModel>.Initialize(entityFrameworkDataContext);
            _hasInitialized = true;
        }
    }

    // Private Methods
    private static void Initialize(IEntityFrameworkDataContext entityFrameworkDataContext)
    {
        var entityType = entityFrameworkDataContext.GetEntityType<TDataModel>();
        if (entityType is null)
            throw new InvalidOperationException(string.Format(ENTITY_TYPE_NOT_FOUND_MESSAGE, typeof(TDataModel).FullName));

        Initialize(
            dataModelType: typeof(TDataModel),
            entityType.GetSchema(),
            entityType.GetTableName()!,
            mappedPropertyMetadadaCollection: entityType.GetProperties().Select(p =>
                new MappedPropertyMetadada(
                    PropertyInfo: p.PropertyInfo ?? throw new InvalidOperationException(),
                    PropertyName: p.Name,
                    ColumnName: p.GetColumnName(),
                    ColumnType: p.ClrType,
                    NpgsqlDbType: GetNpgsqlDbType(p.ClrType),
                    IsPrimaryKey: p.IsPrimaryKey(),
                    IsForeignKey: p.IsForeignKey(),
                    IsNullable: p.IsColumnNullable()
                )
            ).ToArray()
        );
    }
    private async Task GenerateNpgsqlBinaryImporterAsync(CancellationToken cancellationToken)
    {
        if (CurrentNpgsqlBinaryImporter != null)
            return;

        if (EntityFrameworkDataContext.GetDbConnection() is not NpgsqlConnection connection)
            throw new InvalidOperationException(CONNECTION_MUST_BE_NPGSQLCONNECTION_MESSAGE);

        if (!MappedMetadataDictionary.TryGetValue(typeof(TDataModel), out MappedMetadata? mappedMetadata))
            throw new InvalidOperationException(DATA_MODEL_MUST_BE_INITIALIZED);

        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        CurrentNpgsqlBinaryImporter = await connection.BeginBinaryImportAsync(mappedMetadata.BinaryImportCommand, cancellationToken);
    }
    private MappedMetadata GetCurrentMapperMetadata()
    {
        if(_currentMappedMetadata is null)
            _currentMappedMetadata = MappedMetadataDictionary[typeof(TDataModel)];

        return _currentMappedMetadata;
    }

    // Public Methods
    public async Task AddAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        if (EntityFrameworkDataContext.IsBulkInsertOperation)
        {
            await GenerateNpgsqlBinaryImporterAsync(cancellationToken);

            var mappedMetadata = GetCurrentMapperMetadata();

            await CurrentNpgsqlBinaryImporter!.StartRowAsync(cancellationToken);
            foreach (var mappedPropertyMetadada in mappedMetadata.MappedPropertyMetadadaCollection)
            {
                var value = mappedPropertyMetadada.PropertyInfo.GetValue(dataModel);

                if (value is null)
                    await CurrentNpgsqlBinaryImporter.WriteNullAsync(cancellationToken);
                else
                    await CurrentNpgsqlBinaryImporter.WriteAsync(value, mappedPropertyMetadada.NpgsqlDbType, cancellationToken);
            }

            _hasBulkedData = true;
        }
        else
        {
            await DbSet.AddAsync(dataModel, cancellationToken);
        }
    }
    public ValueTask<EntityEntry<TDataModel>> UpdateAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        var entry = EntityFrameworkDataContext.SetEntry(dataModel);
        entry.CurrentValues.SetValues(dataModel);

        return ValueTask.FromResult(entry);
    }
    public async ValueTask<EntityEntry<TDataModel>> AddOrUpdateAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        var existingDataModel = await DbSet.FindAsync(new object?[] { dataModel.Id }, cancellationToken);

        return existingDataModel == null
            ? await DbSet.AddAsync(dataModel, cancellationToken)
            : await UpdateAsync(dataModel, cancellationToken);
    }

    public ValueTask<EntityEntry<TDataModel>> RemoveAsync(TDataModel dataModel, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(DbSet.Remove(dataModel));
    }
    public Task<(bool success, int removeCount)> RemoveAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task<(bool success, int removeCount)> RemoveAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TDataModel> Get(Func<TDataModel, bool> expression)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(expression);
        var localResult = DbSet.Local.Where(expression);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

        return merged;
    }

    public Task<TDataModel?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(q => q.TenantId == tenantId && q.Id == id);
        var localResult = DbSet.Local.Where(q => q.TenantId == tenantId && q.Id == id);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

        return Task.FromResult(merged.FirstOrDefault());
    }
    public Task<IEnumerable<TDataModel>> GetAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(expression);
        var localResult = DbSet.Local.Where(expression);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

        return Task.FromResult(merged);
    }

    public Task<TDataModel?> GetFirstOrDefaultAsync(Func<TDataModel, bool> expression, CancellationToken cancellationToken)
    {
        if(EntityFrameworkDataContext.IsBulkInsertOperation)
            return Task.FromResult(default(TDataModel));
        else
        {
            var dbSetResult = DbSet.AsNoTracking().Where(expression);
            var localResult = DbSet.Local.Where(expression);

            var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

            return Task.FromResult(merged.FirstOrDefault());
        }
    }

    public IEnumerable<TDataModel> GetAll(Guid tenantId)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(q => q.TenantId == tenantId);
        var localResult = DbSet.Local.Where(q => q.TenantId == tenantId);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id);

        return merged;
    }
    public Task<IEnumerable<TDataModel>> GetAllAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var dbSetResult = DbSet.AsNoTracking().Where(q => q.TenantId == tenantId);
        var localResult = DbSet.Local.Where(q => q.TenantId == tenantId);

        var merged = dbSetResult.UnionBy(localResult, keySelector: q => q.Id).AsEnumerable();

        return Task.FromResult(merged);
    }
    public async Task WriteBulkAsync(CancellationToken cancellationToken)
    {
        if (!_hasBulkedData)
            return;

        await PostgreSqlResiliencePolicy.ExecuteAsync(
            handler: async cancellationToken =>
            {
                if (CurrentNpgsqlBinaryImporter is null)
                    throw new InvalidOperationException(NPGSQL_BINARY_IMPORTER_CANNOT_BE_NULL);

                await CurrentNpgsqlBinaryImporter.CompleteAsync(cancellationToken);
            },
            cancellationToken
        );
    }
}
