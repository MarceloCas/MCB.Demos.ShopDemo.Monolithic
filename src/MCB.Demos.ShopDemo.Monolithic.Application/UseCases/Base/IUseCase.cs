using MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base.Input;

namespace MCB.Demos.ShopDemo.Monolithic.Application.UseCases.Base;

public interface IUseCase<in TUseCaseInput>
    where TUseCaseInput : UseCaseInputBase
{
    Task<bool> ExecuteAsync(TUseCaseInput useCaseInput, CancellationToken cancellationToken);
}