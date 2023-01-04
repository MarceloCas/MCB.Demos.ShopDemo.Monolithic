using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Customers.Models;

public class CustomerDto
    : DtoBase
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}
