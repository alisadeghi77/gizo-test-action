namespace Gizo.Domain.Contracts.Base;

public interface ICreateDate<TCreatorId>
{
    DateTime CreateDate { get; set; }
    TCreatorId CreatorId { get; set; }
}

public interface ICreateDate
{
    DateTime CreateDate { get; }
}
