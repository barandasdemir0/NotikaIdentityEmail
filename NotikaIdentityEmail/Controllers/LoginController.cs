using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    
    public class LoginController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailContext _emailContext;

        public LoginController(SignInManager<AppUser> signInManager, EmailContext emailContext)
        {
            _signInManager = signInManager;
            _emailContext = emailContext;
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginViewModel model)
        {

            var value = _emailContext.Users.Where(x => x.UserName == model.Username).FirstOrDefault();
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

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            if (result.Succeeded)
            {
                return RedirectToAction("EditProfile", "Profile");
            }
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış");
            return View(model);





            //return View();

        }
    }
}
