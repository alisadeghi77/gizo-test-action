using System.Linq.Expressions;
using Gizo.Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Extensions;

public static class UserManagerExtensions
{
    public static async Task<User?> FindUserByName<TProperty>(
        this UserManager<User> userManager,
        string username,
        Expression<Func<User, TProperty>> includeMember)
    {
        var normalizeUserName = userManager.KeyNormalizer is null ? username : userManager.NormalizeName(username);

        var user = await userManager.Users
            .Where(_ => _.NormalizedUserName == normalizeUserName)
            .Include(includeMember)
            .FirstOrDefaultAsync();

        return user;
    }
}
