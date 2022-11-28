using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.UserProfileAggregate;

public class UserProfile: BaseEntity<long>
{
    private UserProfile()
    {
    }
    public string IdentityId { get; private set; }
    public BasicInfo BasicInfo { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }

    // Factory method
    public static UserProfile CreateUserProfile(string identityId, BasicInfo basicInfo)
    {
        return new UserProfile
        {
            IdentityId = identityId,
            BasicInfo = basicInfo,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    //public methods

    public void UpdateBasicInfo(BasicInfo newInfo)
    {
        BasicInfo = newInfo;
        LastModified = DateTime.UtcNow;
    }
}
