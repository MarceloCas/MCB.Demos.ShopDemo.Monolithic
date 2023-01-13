namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin.Models;

public class ResiliencePolicyDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string CircuitState { get; set; } = null!;
    public int CurrentCircuitBreakerOpenCount { get; set; }
}
