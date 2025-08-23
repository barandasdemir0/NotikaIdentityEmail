using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class UserController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly EmailContext _emailContext;

        public UserController(UserManager<AppUser> userManager, EmailContext emailContext)
        {
            _userManager = userManager;
            _emailContext = emailContext;
        }

        public async Task<IActionResult> UserList()
        {
            var values = await _userManager.Users.ToListAsync();
            return View(values);
        }

        public async Task<IActionResult> ActiveUser(string id)
        {
            var values = await _userManager.FindByIdAsync(id);
            values.IsActive = true;
            await _userManager.UpdateAsync(values);
            return RedirectToAction("UserList");
        }
        public async Task<IActionResult> PassiveUser(string id)
        {
            var values = await _userManager.FindByIdAsync(id);
            values.IsActive = false;
            await _userManager.UpdateAsync(values);
            return RedirectToAction("UserList");
        }
    }
}
