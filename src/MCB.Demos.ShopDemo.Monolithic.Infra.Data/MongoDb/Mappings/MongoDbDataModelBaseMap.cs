using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels.Base;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings;

public class MongoDbDataModelBaseMap
    : IMongoDbDataModelMap<MongoDbDataModelBase>
{
    public void Map()
    {
        BsonClassMap.RegisterClassMap<MongoDbDataModelBase>(classMap =>
        {
            classMap.MapIdMember(dataModel => dataModel.Id)
                .SetSerializer(new GuidSerializer(BsonType.String));

            classMap.MapMember(dataModel => dataModel.TenantId)
                .SetSerializer(new GuidSerializer(BsonType.String));

            classMap.MapMember(dataModel => dataModel.CreatedAt);
            classMap.MapMember(dataModel => dataModel.CreatedBy);

            classMap.MapMember(dataModel => dataModel.LastUpdatedAt);
            classMap.MapMember(dataModel => dataModel.LastUpdatedBy);

            classMap.MapMember(dataModel => dataModel.LastSourcePlatform);

            classMap.MapMember(dataModel => dataModel.RegistryVersion);

            classMap.MapExtraElementsMember(dataModel => dataModel.ExtraElements);
            classMap.SetDiscriminator(nameof(MongoDbDataModelBase));
        });
    }
}