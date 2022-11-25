namespace Gizo.Domain.Contracts.Base;

public class PagingResult<T>
{
    public IEnumerable<T> Data { get; set; }
    public long Count { get; set; }
}
