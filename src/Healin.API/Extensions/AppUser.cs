using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Healin.API.Extensions
{
    public class AppUser : IAppUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid UserId => IsAuthenticated ? Guid.Parse(_accessor.HttpContext?.User?.GetUserId() ?? Guid.Empty.ToString()) : Guid.Empty;

        public bool IsAuthenticated => _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext?.User?.Claims;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
