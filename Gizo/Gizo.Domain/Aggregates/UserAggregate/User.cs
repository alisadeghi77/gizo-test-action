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

    private readonly List<UserVerificationCode> _userVerificationCodes = new();

    public IReadOnlyCollection<UserVerificationCode> UserVerificationCodes => _userVerificationCodes;

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

    public bool ValidateCode(TimeSpan expirationDurationTime, string code, VerificationType verificationType)
    {
        var data = DateTime.UtcNow.Add(expirationDurationTime * -1);
        var result = UserVerificationCodes
            .Where(_ => _.CreateDate >= data)
            .Where(_ => _.VerificationType == verificationType)
            .Any(_ => _.Code == code);

        if (result)
            _userVerificationCodes.RemoveAll(_ => _.VerificationType == verificationType);

        return result;
    }

    public UserVerificationCode? GetValidCode(TimeSpan expirationDurationTime, VerificationType verificationType)
    {
        var date = DateTime.UtcNow.Add(expirationDurationTime * -1);

        var verifyCode = UserVerificationCodes
            .Where(_ => _.CreateDate >= date)
            .Where(_ => _.VerificationType == verificationType)
            .MinBy(_ => _.CreateDate);

        return verifyCode;
    }

    public UserVerificationCode CreateCode(bool useSampleCode, VerificationType verificationType)
    {
        var verifyCode = UserVerificationCode.Create(Id, useSampleCode, verificationType);
        _userVerificationCodes.Add(verifyCode);
        return verifyCode;
    }

    public static User CreateUserByPhoneNumber(string phoneNumber)
    {
        return new User()
        {
            PhoneNumber = phoneNumber,
            UserName = phoneNumber
        };
    }
}