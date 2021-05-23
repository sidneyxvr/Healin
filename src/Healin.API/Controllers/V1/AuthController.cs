using Healin.API.Extensions;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Application.Requests.Auth;
using Healin.Application.Responses.Auth;
using Healin.Infrastructure.Identity.Models;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly AppSettings _appSettings;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IPatientService patientService,
            IDoctorService doctorService,
            IOptions<AppSettings> appSettings,
            INotifier notifier, IAppUser appUser) : base(notifier, appUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _patientService = patientService;
            _doctorService = doctorService;
            _appSettings = appSettings?.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public async Task<ActionResult<UserTokenResponse>> Login(LoginRequest login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJwt(login.Email));
            }

            if (result.IsLockedOut)
            {
                NotifyError("Usuário temporariamente bloqueado");
                return CustomResponse();
            }

            NotifyError("Usuário ou senha inválido");
            return CustomResponse();
        }

        [HttpPost("doctor")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public async Task<ActionResult<UserTokenResponse>> RegisterDoctor(DoctorRequest doctor)
        {
            var user = new ApplicationUser
            {
                Email = doctor.Email,
                UserName = doctor.Email,
                EmailConfirmed = true
            };
            
            var result = await _userManager.CreateAsync(user, doctor.Password);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => NotifyError(error.Description));
                return CustomResponse();
            }

            doctor.Id = user.Id;
            await _doctorService.AddAsync(doctor);

            if (!IsValid())
            {
                await _userManager.DeleteAsync(user);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
            }

            return CustomResponse();
        }

        [HttpPost("patient")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public async Task<ActionResult<UserTokenResponse>> RegisterPatient(PatientRequest patient)
        {
            if(patient is null)
            {
                NotifyError("Paciente inválido");
                return CustomResponse();
            }

            var user = new ApplicationUser
            {
                Email = patient.Email,
                UserName = patient.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, patient.Password);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => NotifyError(error.Description));
                return CustomResponse();
            }

            patient.Id = user.Id;
            await _patientService.AddAsync(patient);

            if (!IsValid())
            {
                await _userManager.DeleteAsync(user);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Patient");
            }

            return CustomResponse();
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetInfo()
        {
            return CustomResponse(
                User.IsInRole("Patient") ? 
                    await _patientService.GetByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))) : 
                    await _doctorService.GetByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordRequest updatePasswordRequest)
        {
            if(User.FindFirstValue(ClaimTypes.Email) != updatePasswordRequest.Email)
            {
                return Forbid();
            }

            var user = await _userManager.FindByEmailAsync(updatePasswordRequest.Email);

            if(user is null)
            {
                NotifyError("Usuário não encontrado");
                return CustomResponse();
            }

            var result = await _userManager.ChangePasswordAsync(user, updatePasswordRequest.Password, updatePasswordRequest.NewPassword);

            result.Errors.ToList().ForEach(e => NotifyError(e.Description));

            return CustomResponse();
        }


        [HttpPut("deactivate")]
        public async Task<IActionResult> Deactivate()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if (user is null)
            {
                NotifyError("Usuário não encontrado");
                return CustomResponse();
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));

            result.Errors.ToList().ForEach(e => NotifyError(e.Description));

            if (User.IsInRole("Patient"))
            {
                await _patientService.DisableAsync(user.Id);
            }
            else if (User.IsInRole("Doctor"))
            {
                await _doctorService.DisableAsync(user.Id);
            }

            if (!IsValid())
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddDays(-1));
            }

            return CustomResponse();
        }
        private async Task<UserLoginResponse> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = (await _userManager.GetRolesAsync(user))
                .Select(role => new Claim("role", role))
                .ToList();

            var identityClaims = await GetUserClaimsAsync(claims, user);
            var encodedToken = CodifyToken(identityClaims);

            return GetTokenResponse(encodedToken, user, claims);
        }

        private static async Task<ClaimsIdentity> GetUserClaimsAsync(ICollection<Claim> claims, ApplicationUser user)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString(CultureInfo.CurrentCulture)));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(CultureInfo.CurrentCulture), ClaimValueTypes.Integer64));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return await Task.FromResult(identityClaims);
        }

        private string CodifyToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emitter,
                Audience = _appSettings.ValidOn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.HourValidation),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private UserLoginResponse GetTokenResponse(string encodedToken, ApplicationUser user, IEnumerable<Claim> claims)
        {
            return new UserLoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.HourValidation).TotalSeconds,
                UserToken = new UserTokenResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaimResponse { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
