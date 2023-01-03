namespace Gizo.Domain.Contracts.Base;

public interface IEntity
{
}

public interface IEntity<T>
{
    T Id { get; set; }
}
