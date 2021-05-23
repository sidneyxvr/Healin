using System;
using System.Collections.Generic;

namespace Healin.Application.Responses.Auth
{
    public class UserTokenResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaimResponse> Claims { get; set; }
    }
}
