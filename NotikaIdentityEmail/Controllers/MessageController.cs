using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            // var messages = _context.Messages.Where(x => x.ReceiverEmail == values.Email).ToList();


            var messages = (from x in _context.Messages
                            join u in _context.Users
                            on x.SenderEmail equals u.Email into userGroup
                            from sender in userGroup.DefaultIfEmpty()
                            join c in _context.Categories
                            on x.CategoryID equals c.CategoryID into categoryGroup
                            from category in categoryGroup.DefaultIfEmpty()
                            where
                            x.ReceiverEmail == values.Email
                            select new MessageWithSenderInfoViewModel
                            {
                                MessageId = x.MessageID,
                                MessageDetail = x.MessageDetail,
                                Subject = x.Subject,
                                SendDate = x.SendDate,
                                SenderMail = x.SenderEmail,
                                SenderName = sender != null ? sender.Name : "Bilinmeyen",
                                SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                                CategoryName = category != null ? category.CategoryName : "Bilinmeyen Kategori"
                            }).ToList();
            return View(messages);
            //return View();
        }
        public async Task<IActionResult> Sendbox()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            // var messages = _context.Messages.Where(x => x.ReceiverEmail == values.Email).ToList();


            var messages = (from x in _context.Messages
                            join u in _context.Users
                            on x.ReceiverEmail equals u.Email into userGroup
                            from receiver in userGroup.DefaultIfEmpty()
                            join c in _context.Categories
                            on x.CategoryID equals c.CategoryID into categoryGroup
                            from category in categoryGroup.DefaultIfEmpty()
                            where
                            x.SenderEmail == values.Email
                            select new MessageWithReceiverInfoViewModel
                            {
                                MessageId = x.MessageID,
                                MessageDetail = x.MessageDetail,
                                Subject = x.Subject,
                                SendDate = x.SendDate,
                                ReceiverMail = x.ReceiverEmail,
                                ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
                                ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
                                CategoryName = category != null ? category.CategoryName : "Bilinmeyen Kategori"
                            }).ToList();
            return View(messages);
        }

        public IActionResult MessageDetail(int id)
        {
            var messages = _context.Messages.Where(x => x.MessageID == id).FirstOrDefault();
            return View(messages);
            //return View();
        }

        [HttpGet]
        public IActionResult ComposeMessage()
        {

            var categories = _context.Categories.ToList();
            ViewBag.v = categories.Select(x => new SelectListItem
            {
                Value = x.CategoryID.ToString(),
                Text = x.CategoryName
            }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ComposeMessage(Entities.Message message)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            message.SenderEmail = values.Email;
            message.SendDate = DateTime.Now;
            message.IsRead = false;

            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Sendbox");
        }
    }
}
