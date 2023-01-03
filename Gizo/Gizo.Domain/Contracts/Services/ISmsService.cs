namespace Gizo.Domain.Contracts.Services;

public interface ISmsService
{
    Task Send(string phoneNumber, string body);
}
