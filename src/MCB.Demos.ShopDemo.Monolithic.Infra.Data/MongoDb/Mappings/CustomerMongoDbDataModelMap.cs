using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings.Base;
using MongoDB.Bson.Serialization;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.Mappings;

public class CustomerMongoDbDataModelMap
    : IMongoDbDataModelMap<CustomerMongoDbDataModel>
{
    public void Map()
    {
        BsonClassMap.RegisterClassMap<CustomerMongoDbDataModel>(classMap =>
        {
            classMap.MapMember(dataModel => dataModel.FirstName);
            classMap.MapMember(dataModel => dataModel.LastName);
            classMap.MapMember(dataModel => dataModel.BirthDate);
            classMap.MapMember(dataModel => dataModel.Email);

            classMap.SetDiscriminator(nameof(CustomerMongoDbDataModel));
        });
    }
}