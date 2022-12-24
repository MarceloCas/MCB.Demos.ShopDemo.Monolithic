using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModels;
using MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.EntityFramework.DataModelsRepositories.Interfaces;
public interface ICustomerDataModelRepository
    : IDataModelRepository<CustomerDataModel>
{
}
