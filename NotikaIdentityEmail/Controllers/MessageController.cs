using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Inbox()
        {
            return View();
        }
    }
}
