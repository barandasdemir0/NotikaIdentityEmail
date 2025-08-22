using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        //bunun sayesinde gizli sadece okunabilir usermanager ıdentityu kütüphanesi ile gelir içine app user tanımlı sınıfımız geldi  ve buradan field yani erişim kullanacağımız değişken ürettik



        // bu ise yapıcı metottur yani örnek veriyoruz ilk veriyi hazırlar mesela başladık neye başladık bir öğrenci verdik ad soyad numara burası ctor ile yani consturctor ad baran soyad daşdemir numara 111 numarası atadı 
        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        //bu yapının tamamı dependenjy injection oldu kahve mantığı gibi düşün kahve makinesinin türk kahvesi öğütücüsü koyarsan türk kahvesi olur espresso öğütücü koyarsan espresso olur işte bu öğütücü dependecy injectiondur



        //bunların hepsi asenkron çalışırsa olur senkron kodu yukarıdan aşağıya okur ama asenkron ile işlem yapılırken diğer işlemlerde yapılır örnek senkron garson sipariş alır başka işlem yapmaz asenkron ise garson sipariş alır başka müşteridende şipariş alır
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        //bir metodun birden fazla aynı isimde farklı şekillerde çağırılmasına overloading denir burası overloadingtir
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
        {

            Random random = new();
            //random sınıfından random bir nesne oluşturduk
            //random sınıfı random sayılar üretir
            int activationCode = random.Next(100000, 999999);
            //random sınıfından random bir nesne oluşturduk ve 100000 ile 999999 arasında rastgele bir sayı ürettik
            AppUser appUser = new AppUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
                ActivationCode = activationCode,
            };
            var result = await _userManager.CreateAsync(appUser, model.Password);// appuser türünde parametre istiyor // burada oluşturma işlemi yaptık
            if (result.Succeeded)// bu işlemler başarılı olursa
            {
                //kullanıcıyı aktif etme işlemi için activation kodu gönderilecek  kurzpwyuuaybvfzf

                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress("NotikaAdminChat", "ebtuyou1@gmail.com");
                mimeMessage.From.Add(mailboxAddressFrom); // kimden geldiğini belirtiyoruz yani kullanıcıya gözükecek mail

                MailboxAddress mailboxAddressTo = new MailboxAddress("User", model.Email);//bir kullanıcı ve modeldeki email adresi
                mimeMessage.To.Add(mailboxAddressTo); // kime gideceğini belirtiyoruz yani kullanıcıya gidecek mail

                var bodybuilder = new BodyBuilder();
                bodybuilder.TextBody = $"Merhaba {model.Name} {model.Surname},\n\n" +
                                       $"Hesabınızı aktifleştirmek için lütfen aşağıdaki kodu kullanın:\n\n" +
                                       $"{activationCode}\n\n" +
                                       "NotikaAdminChat Ekibi\n\n" + $"Hello {model.Name} {model.Surname},\n\n" +
                                        $"Please use the following code to activate your account:\n\n" +
                                        $"{activationCode}\n\n" +
                                        "NotikaAdminChat Team";

                mimeMessage.Body = bodybuilder.ToMessageBody(); // mailin içeriğini belirtiyoruz

                mimeMessage.Subject = "Notika Hesap Aktivasyon Kodu   -     Notika Account Activation Code    "; // mailin konusunu belirtiyoruz


                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect("smtp.gmail.com", 587,false); // gmail smtp sunucusuna bağlanıyoruz
                smtpClient.Authenticate("ebtuyou1@gmail.com", "kurzpwyuuaybvfzf"); // gmail hesabımızın kullanıcı adı ve şifresi ile kimlik doğrulaması yapıyoruz

                smtpClient.Send(mimeMessage); // maili gönderiyoruz
                smtpClient.Disconnect(true); // bağlantıyı kesiyoruz




                TempData["EmailMove"] = model.Email; // email adresini tempdata'ya atıyoruz



                return RedirectToAction("UserActivation", "Activation"); //buraya gönder
            }
            else //başarısız
            {
                foreach (var item in result.Errors) // eğer başarırsız olursa hataları döndür errordan gelen hataları
                {
                    ModelState.AddModelError("", item.Description);// key ve mesaj
                }
            }
            return View();
        }


        //tasklar geri dönüş yapar await ile çalışır ve uygulamayı dondurmaz kodun başka işlemleri engellemeden yapmasını sağlar
        // örnek vermek gerekirse bilgisayarda 2 işlem yaparsın 1ini arkaya atarsın burada task olur
        //await işe uygulamanın donmamasını sağlar
    }
}
