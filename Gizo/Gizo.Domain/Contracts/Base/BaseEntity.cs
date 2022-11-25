namespace Gizo.Domain.Contracts.Base;

public abstract class BaseEntity<T> : IEntity<T>, IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{ }

public interface IEntity<T>
{
    T Id { get; set; }
}

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}

public interface IRegisterDate<TRegistrarId>
{
    DateTime RegisterDate { get; set; }
    TRegistrarId RegistrarId { get; set; }
}

public interface IModifyDate<TModifierId>
{
    DateTime ModifyDate { get; set; }
    TModifierId ModifierId { get; set; }
}

public interface IOptionalModifiedDate<TModifierId>
    where TModifierId : struct
{
    DateTime? ModifyDate { get; set; }
    TModifierId? ModifierId { get; set; }
}
