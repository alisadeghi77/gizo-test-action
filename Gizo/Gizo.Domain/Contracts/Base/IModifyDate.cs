namespace Gizo.Domain.Contracts.Base;

public interface IModifyDate<TModifierId>
{
    DateTime ModifyDate { get; set; }
    TModifierId ModifierId { get; set; }
}