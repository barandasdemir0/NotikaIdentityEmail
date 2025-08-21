using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;

        public MessageController(EmailContext context)
        {
            _context = context;
        }

        public IActionResult Inbox()
        {
            var messages = _context.Messages.Where(x => x.ReceiverEmail == "ali@testcom").ToList();
             return View(messages);
            //return View();
        }
        public IActionResult Sendbox()
        {
            var messages = _context.Messages.Where(x => x.SenderEmail == "ali@testcom").ToList();
             return View(messages);
            //return View();
        }

        public IActionResult MessageDetail()
        {
            var messages = _context.Messages.Where(x => x.MessageID == 1).FirstOrDefault();
             return View(messages);
            //return View();
        }

        public IActionResult ComposeMessage()
        {
                       return View();
        }
    }
}
