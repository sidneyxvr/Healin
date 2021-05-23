using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Healin.Shared.Intefaces
{
    public interface IAppUser
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
