using MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.DataModels;
public class ProductDataModel
    : DataModelBase
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
}
