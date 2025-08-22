using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    [Authorize]

    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name); // giriş yapan kullanıcının bilgilerini al
            UserEditViewModel model = new UserEditViewModel();

            model.Name = values.Name;
            model.Surname = values.Surname;
            model.Username = values.UserName;
            model.City = values.City;
            model.ImageUrl = values.ImageUrl;
            model.PhoneNumber = values.PhoneNumber;
            model.Email = values.Email;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            if (model.Password == model.PasswordConfirm)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.Name = model.Name;
                user.Surname = model.Surname;   
                user.UserName = model.Username;
                user.City = model.City;
                user.ImageUrl = model.ImageUrl;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;

               user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                 await _userManager.UpdateAsync(user);
               

            }
            return View();
        }
    }
}
