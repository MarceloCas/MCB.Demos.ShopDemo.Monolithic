namespace MCB.Demos.ShopDemo.Monolithic.Infra.CrossCutting.Settings.Models;
public class Consul
{
    // Properties
    public string Address { get; set; } = null!;
    public int RefreshIntervalInSeconds { get; set; }

    // Public Methods
    public (bool IsValid, IEnumerable<string> Messages) Validate()
    {
        var messageCollection = new List<string>();

        if (string.IsNullOrEmpty(Address))
            messageCollection.Add($"{typeof(Consul).FullName}.{nameof(Address)} cannot be null");

        return (IsValid: messageCollection.Count == 0, Messages: messageCollection);
    }
}
