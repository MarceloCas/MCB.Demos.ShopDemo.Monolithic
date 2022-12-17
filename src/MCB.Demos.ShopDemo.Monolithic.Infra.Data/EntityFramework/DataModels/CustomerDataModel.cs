﻿using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels;
public class CustomerDataModel
    : DataModelBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = null!;
}
