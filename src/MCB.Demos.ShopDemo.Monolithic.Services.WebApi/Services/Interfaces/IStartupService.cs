namespace MCB.Demos.ShopDemo.Monolithic.Services.WebApi.Services.Interfaces;

public interface IStartupService
{
    // Properties
    bool HasStarted { get; }

    // Methods
    Task<(bool Success, string[]? Messages)> TryStartupApplicationAsync(CancellationToken cancellationToken);
}
