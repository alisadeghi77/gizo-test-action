using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class UserVerificationCode : IEntity, IEntity<long>, ICreateDate
{
    private UserVerificationCode(long userId, string code, VerificationType verificationType)
    {
        UserId = userId;
        Code = code;
        VerificationType = verificationType;
        CreateDate = DateTime.UtcNow;
    }
    public long Id { get; set; }
    public string Code { get; init; }
    public long UserId { get; init; }
    public DateTime CreateDate { get; set; }
    public VerificationType VerificationType { get; init; }

    public static UserVerificationCode Create(long userId, bool useSampleCode, VerificationType verificationType)
    {
        var code = useSampleCode ? "11111" : new Random().Next(10000, 99999).ToString();

        return new UserVerificationCode(userId, code, verificationType);
    }
}
