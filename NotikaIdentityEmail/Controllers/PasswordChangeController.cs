using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.ForgetPasswordModels;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class PasswordChangeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public PasswordChangeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetTokenLink = Url.Action("ResetPassword", "PasswordChange", new
            {
                userId = user.Id,
                
                token = passwordResetToken
            }, HttpContext.Request.Scheme);



          
            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress("Notika Admin", "ebtuyou1@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();


            mimeMessage.Subject = "Şifre Sıfırlama Talebi";



            bodyBuilder.TextBody = $"Merhaba,\n\n" +
                                   $"Hesabınızın Şifresini değiştirmek için lütfen aşağıdaki alanı kullanın:\n\n" +
                                   $"{passwordResetTokenLink}\n\n" +
                                   "NotikaAdminChat Ekibi\n\n" + $"Hello \n\n" +
                                    $"To change your account password, please use the field below.:\n\n" +
                                    $"{passwordResetTokenLink}\n\n" +
                                    "NotikaAdminChat Team";

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false); // gmail smtp sunucusuna bağlanıyoruz
            smtpClient.Authenticate("ebtuyou1@gmail.com", "kurzpwyuuaybvfzf"); // gmail hesabımızın kullanıcı adı ve şifresi ile kimlik doğrulaması yapıyoruz

            smtpClient.Send(mimeMessage); // maili gönderiyoruz
            smtpClient.Disconnect(true); // bağlantıyı kesiyoruz


            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId== null || token==null)
            {
                ModelState.AddModelError("", "Hata oluştur");
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ResetPasswordAsync(user, token.ToString(), model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("UserLogin", "Login");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                 
                }
                return View();
            }

        }







    }
}
