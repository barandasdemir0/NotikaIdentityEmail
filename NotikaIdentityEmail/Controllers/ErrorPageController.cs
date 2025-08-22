using Microsoft.AspNetCore.Mvc;

namespace NotikaIdentityEmail.Controllers
{
    public class ErrorPageController : Controller
    {

        [Route("Error/404")]
        public IActionResult Page404()
        {
            return View();
        }

        [Route("Error/401")]
        public IActionResult Page401()
        {
            return View();
        }
        [Route("Error/403")]
        public IActionResult Page403()
        {
            return View();
        }
        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int code)
        {
            if (code==404)
            {
                return RedirectToAction("Page404");
            }
            if (code==401)
            {
                return RedirectToAction("Page401");
            }
            if (code==403)
            {
                return RedirectToAction("403");
            }
            return View(code);
        }
    }
}
