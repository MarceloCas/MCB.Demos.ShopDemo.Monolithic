namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Models;
public record MappedMetadata(
    string? SchemaName,
    string TableName,
    MappedPropertyMetadada[] MappedPropertyMetadadaCollection,
    string BinaryImportCommand
);
