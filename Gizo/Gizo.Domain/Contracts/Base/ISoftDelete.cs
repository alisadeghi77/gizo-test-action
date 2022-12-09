namespace Gizo.Domain.Contracts.Base;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}