using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.ViewComponents
{
    public class _HeaderUserLayoutComponentPartial:ViewComponent
    {
        private readonly EmailContext _emailContext;
        private readonly UserManager<AppUser> _userManager;

        public _HeaderUserLayoutComponentPartial(UserManager<AppUser> userManager, EmailContext emailContext)
        {
            _userManager = userManager;
            _emailContext = emailContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            var userMail = values.Email;
            ViewBag.Count = _emailContext.Messages.Count(x => x.ReceiverEmail == userMail && x.IsRead == false);
            ViewBag.NotificationCount = _emailContext.Notifications.Count();
            return View();
        }
    }
    
}
