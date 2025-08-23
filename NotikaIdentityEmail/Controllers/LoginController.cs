using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;
using NotikaIdentityEmail.Models.JwtModels;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    
    public class LoginController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettingsModel _jwtSettingsModel;

        public LoginController(SignInManager<AppUser> signInManager, EmailContext emailContext, UserManager<AppUser> userManager, IOptions< JwtSettingsModel> jwtSettingsModel)
        {
            _signInManager = signInManager;
            _emailContext = emailContext;
            _userManager = userManager;
            _jwtSettingsModel = jwtSettingsModel.Value;
        }


        [HttpGet]
        public IActionResult LoginWithGoogle()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginViewModel model)
        {

            var value = _emailContext.Users.FirstOrDefault(x => x.UserName == model.Username);

            SimpleUserViewModel simpleUser = new SimpleUserViewModel()
            {
                Name = value.Name,
                Surname = value.Surname,
                Id = value.Id,
                Email = value.Email,
                City = value.City,
                Username = value.UserName
            };


            if (value == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            if (!value.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "E-Mail Adresinizi henüz onaylanmamış.");
                return View(model);
            }
            if (!value.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı pasif Durumunda Giriş Yapamaz.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            if (result.Succeeded)
            {

              
                var token = GenerateJwtToken(simpleUser);
                Response.Cookies.Append("jwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes)
                });
                return RedirectToAction("EditProfile", "Profile");
            }
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış");
            return View(model);





        
        }


        [HttpPost]
        public string GenerateJwtToken(SimpleUserViewModel model)
        {
            var claim = new[]
            {
                new Claim("name",model.Name),
                new Claim("Surname",model.Surname),
                new Claim("Username",model.Username),
                new Claim("City",model.City),
                new Claim(ClaimTypes.NameIdentifier,model.Id),
                new Claim(ClaimTypes.Email,model.Email),

                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //token ürettik
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingsModel.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettingsModel.Issuer,
                audience: _jwtSettingsModel.Audience,
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes),
                signingCredentials: creds);
            //model.Token = new JwtSecurityTokenHandler().WriteToken(token);
            //return View(model);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost]
        public IActionResult ExternalLogin(string provider , string? returnUrl=null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Login", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string? returnUrl=null,string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"External Provider Error: {remoteError}");
                return RedirectToAction("Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info==null)
            {
                return RedirectToAction("Login");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Inbox", "Message");
            }
            else  
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new AppUser()
                {
                    UserName = email,
                    Email = email,
                    Name = info.Principal.FindFirstValue(ClaimTypes.GivenName)??"Google",
                    Surname = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User"
                };
                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Inbox", "Message");
                }
                return RedirectToAction("UserLogin");
            }
        }
    }
}
