using System.Security.Claims;

namespace Gizo.Api.Extensions;

public static class HttpContextExtensions
{
    public static long GetUserProfileIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("UserProfileId", context);
    }

    public static long GetIdentityIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("IdentityId", context);
    }

    private static long GetGuidClaimValue(string key, HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        return long.Parse(identity?.FindFirst(key)?.Value ?? "0");
    }
}