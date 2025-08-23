using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.MessageViewModel;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.ViewComponents.NavbarHeaderViewComponent
{
    public class _MessageListOnNavbarHeaderComponenPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _emailContext;



        public _MessageListOnNavbarHeaderComponenPartial(UserManager<AppUser> userManager, EmailContext emailContext)
        {
            _userManager = userManager;
            _emailContext = emailContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var userValue = await _userManager.FindByNameAsync(User.Identity.Name);
            var loginUserValues = userValue.Email;
          
                var values = from message in _emailContext.Messages
                             join user in _emailContext.Users
                             on message.SenderEmail equals user.Email
                             where message.ReceiverEmail == loginUserValues && message.IsRead == false
                             select new MessageListWithUsersInfoViewModel
                             {
                                 Fullname = user.Name + " " + user.Surname,
                                 ProfileImageUrl = user.ImageUrl,
                                 MessageDetail = message.MessageDetail,
                                 SendDate = message.SendDate
                             };


            return View(values.ToList());
        }
    }
}
