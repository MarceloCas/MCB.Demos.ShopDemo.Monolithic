using MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.MongoDb.DataModels;

public class CustomerMongoDbDataModel
    : MongoDbDataModelBase
{
    // Properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }

    // Constructors
    public CustomerMongoDbDataModel()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }
}