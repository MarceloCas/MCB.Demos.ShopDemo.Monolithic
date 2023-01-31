using NpgsqlTypes;
using System.Reflection;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Models;
public record MappedPropertyMetadada(
    PropertyInfo PropertyInfo,
    string PropertyName,
    string ColumnName,
    Type ColumnType,
    NpgsqlDbType NpgsqlDbType,
    bool IsPrimaryKey,
    bool IsForeignKey,
    bool IsNullable
);
