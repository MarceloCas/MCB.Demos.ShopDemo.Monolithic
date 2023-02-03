using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;

public interface IUseCase<in TUseCaseInput, TUseCaseOutput>
    where TUseCaseInput : UseCaseInputBase
{
    Task<(bool Success, TUseCaseOutput? Output)> ExecuteAsync(TUseCaseInput useCaseInput, CancellationToken cancellationToken);
}