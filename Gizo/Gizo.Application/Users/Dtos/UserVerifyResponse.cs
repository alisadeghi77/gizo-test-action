namespace Gizo.Application.Users.Dtos;

public class UserVerifyResponse
{
    public UserVerifyResponse(string userName, string token)
    {
        UserName = userName;
        Token = token;
    }

    public UserVerifyResponse()
    {
    }
    public string UserName { get; set; }

    public string Token { get; set; }
}
