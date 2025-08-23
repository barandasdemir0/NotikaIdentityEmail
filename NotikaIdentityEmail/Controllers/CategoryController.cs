using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace NotikaIdentityEmail.Controllers
{
    public class CategoryController : Controller
    {

        private readonly EmailContext _emailContext;

        public CategoryController(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

      
        public IActionResult Index()
        {

            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                TempData["error"] = "Giriş Yapmalısınız";
                return RedirectToAction("UserLogin", "Login");
            }

            JwtSecurityToken jwt;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                jwt = handler.ReadJwtToken(token);
            }
            catch 
            {

                TempData["error"] = "Token Geçersiz";
                return RedirectToAction("UserLogin", "Login");
            }

            var city = jwt.Claims.FirstOrDefault(c=>c.Type == "City")?.Value;
            if (city!="ankara")
            {
                return Forbid();
            }
            var values = _emailContext.Categories.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            category.CategoryStatus = true;
            _emailContext.Categories.Add(category);
            _emailContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            var values = _emailContext.Categories.Find(id);
            _emailContext.Remove(values);
            _emailContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var values = _emailContext.Categories.Find(id);
            return View(values);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {

            category.CategoryStatus = true;
            _emailContext.Categories.Update(category);
            _emailContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
