using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class User : IdentityUser<long>, IEntity, ICreateDate, IOptionalModifiedDate<long>
{
    public User()
    {
        CreateDate = DateTime.UtcNow;
    }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifyDate { get; set; }

    public long? ModifierId { get; set; }

    public IReadOnlyCollection<Trip> Trips { get; private set; }

    public IReadOnlyCollection<UserCar> UserCars { get; private set; }

    public void UpdateUserProfile(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        NormalizedEmail = email.ToUpper();
        ModifyDate = DateTime.UtcNow;
    }
}
