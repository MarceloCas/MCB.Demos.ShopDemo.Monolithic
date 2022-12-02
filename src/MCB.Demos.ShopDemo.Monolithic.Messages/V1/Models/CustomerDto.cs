using MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models.Base;

namespace MCB.Demos.ShopDemo.Monolithic.Messages.V1.Models;

public class CustomerDto
    : DtoBase
{
    // Properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }

    // Constructors
    public CustomerDto()
        : base()
    {
        FirstName = LastName = Email = string.Empty;
    }
}