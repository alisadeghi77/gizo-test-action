namespace Gizo.Domain.Contracts.Base;

public interface IOptionalModifiedDate<TModifierId>
    where TModifierId : struct
{
    DateTime? ModifyDate { get; set; }
    TModifierId? ModifierId { get; set; }
}

public interface IOptionalModifiedDate
{
    DateTime? ModifyDate { get; set; }
}