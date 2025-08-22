using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotikaIdentityEmail.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotikaIdentityEmail.Controllers
{
    public class TokenController : Controller
    {

        private readonly JwtSettingsModel _jwtSettingsModel;

        public TokenController(IOptions< JwtSettingsModel> jwtSettingsModel)
        {
            _jwtSettingsModel = jwtSettingsModel.Value;
        }

        [HttpGet]
        public IActionResult Generate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Generate(SimpleUserViewModel model)
        {
            var claim = new[]
            {
                new Claim("name",model.Name),
                new Claim("Surname",model.Surname),
                new Claim("Username",model.Username),
                new Claim("City",model.City),
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
            model.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return View(model);
        }
    }
}
