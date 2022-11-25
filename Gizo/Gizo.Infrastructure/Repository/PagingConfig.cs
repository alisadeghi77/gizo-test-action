namespace Gizo.Infrastructure.Repository;

public class PagingConfig
{
    public PagingConfig(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    public int Take { get; set; }
    public int Skip { get; set; }
}
