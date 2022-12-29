namespace Gizo.Domain.Contracts.Base;

public interface IOptionalModifiedDate<TModifierId>
    where TModifierId : struct
{
    DateTime? ModifyDate { get; }
    TModifierId? ModifierId { get; }
}

public interface IOptionalModifiedDate
{
    DateTime? ModifyDate { get; }
}