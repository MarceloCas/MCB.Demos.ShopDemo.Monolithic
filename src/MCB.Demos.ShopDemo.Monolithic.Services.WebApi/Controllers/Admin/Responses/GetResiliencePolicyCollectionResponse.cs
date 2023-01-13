using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Resilience;
using MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin.Models;

namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Controllers.Admin.Responses;

public class GetResiliencePolicyCollectionResponse
{
    public IEnumerable<ResiliencePolicyDto>? ResiliencePolicyCollection { get; set; }
}
