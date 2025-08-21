using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.ViewComponents.MessageViewComponents
{
    public class _MessageCategoryListSidebarComponentPartial:ViewComponent
    {

        private readonly EmailContext _context;

        public _MessageCategoryListSidebarComponentPartial(EmailContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var value = _context.Categories.ToList();
            return View(value);
        }
    }
}
