using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class User : IdentityUser<long>, IEntity, ICreateDate, IOptionalModifiedDate<long>
{
    private readonly List<UserCarModel> _userCarModels = new();

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


    public IReadOnlyCollection<UserCarModel> UserCarModels => _userCarModels;

    public void UpdateProfile(long userId,string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        NormalizedEmail = email.ToUpper();
        ModifyDate = DateTime.UtcNow;
        ModifierId = userId;
    }

    public List<UserCarModel> AddCar(long carModelId, string license)
    {
        var cars = new UserCarModel(Id, carModelId, license);
        _userCarModels.Add(cars);

        return _userCarModels;
    }

    public List<UserCarModel> RemoveCar(UserCarModel userCar)
    {
        _userCarModels.Remove(userCar);

        return _userCarModels;
    }

    public UserCarModel? FindUserCarModel(long carModelId)
    {
        return UserCarModels.FirstOrDefault(_ => _.CarModelId == carModelId);
    }
}
