namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models.RedisConfig;
public class TtlSeconds
{
    // Properties
    public int CustomerDataModel { get; set; }

    // Public Methods
    public static (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        return (IsValid: true, Messages: Enumerable.Empty<string>());
    }
}
