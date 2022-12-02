using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;

namespace MCB.Demos.ShopDemo.Monolithic.Domain.Entities.Base;

public abstract class DomainEntityBase
    : Core.Domain.Entities.DomainEntitiesBase.DomainEntityBase
{
    protected DomainEntityBase(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
    }
}