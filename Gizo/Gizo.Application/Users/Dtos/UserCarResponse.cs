namespace Gizo.Application.Users.Dtos;

public class UserCarResponse
{
    public long Id { get; set; }

    public string? CarName { get; set; }

    public string? CarModel { get; set; }

    public string? License { get; set; }

    public bool IsSelected { get; set; }
}
