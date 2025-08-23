using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.ViewComponents.NavbarHeaderViewComponent
{
    public class _NavbarListOnNavbarHeaderComponenPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _emailContext;

        public _NavbarListOnNavbarHeaderComponenPartial(EmailContext emailContext, UserManager<AppUser> userManager)
        {
            _emailContext = emailContext;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
           var notification =  _emailContext.Notifications.ToList();

            return View(notification);
        }
    }
}
