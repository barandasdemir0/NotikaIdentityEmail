using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
