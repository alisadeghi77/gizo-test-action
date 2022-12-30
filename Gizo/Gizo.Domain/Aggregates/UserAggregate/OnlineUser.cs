using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class OnlineUser : ICreateDate
{
    public OnlineUser() => CreateDate = DateTime.UtcNow;

    public long Id { get; set; }
    public string? ConnectionId { get; set; }
    public string? UserId { get; set; }
    public DateTime ConnectedDate { get; set; }
    public DateTime? DisconnectedDate { get; set; }

    public DateTime CreateDate { get; set; }
}