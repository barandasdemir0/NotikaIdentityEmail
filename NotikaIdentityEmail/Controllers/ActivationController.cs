using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers
{
    public class ActivationController : Controller
    {

        private readonly EmailContext _emailContext;

        public ActivationController(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

        [HttpGet]
        public IActionResult UserActivation()
        {
            var email = TempData["EmailMove"];
            //ViewBag.email = email; // TempData'dan email adresini alıp ViewBag'e atıyoruz
            TempData["test1"] = email; // TempData'dan email adresini alıp tekrar TempData'ya atıyoruz
            return View();
        }
        [HttpPost]
        public IActionResult UserActivation(int userCodeParameter)
        {
            string email = TempData["Test1"].ToString();


         

            var code = _emailContext.Users
                .Where(x => x.Email == email)
                .Select(y => y.ActivationCode)
                .FirstOrDefault();

            if (userCodeParameter == code)
            {
                var value = _emailContext.Users.Where(x => x.Email == email).FirstOrDefault();
                value.EmailConfirmed = true;
                _emailContext.SaveChanges();

                return RedirectToAction("UserLogin", "Login");
            }

            ModelState.AddModelError("", "Aktivasyon kodu hatalı");
            return View();
        }

    }
}
