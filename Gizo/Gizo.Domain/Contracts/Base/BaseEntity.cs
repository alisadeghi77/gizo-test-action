namespace Gizo.Domain.Contracts.Base;

public abstract class BaseEntity<T> : IEntity<T>, IEntity
{
    public T Id { get; set; } = default!;
}
