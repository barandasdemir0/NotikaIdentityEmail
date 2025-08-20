using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;
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
            AppUser appUser = new AppUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email=model.Email,
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(appUser,model.Password);// appuser türünde parametre istiyor // burada oluşturma işlemi yaptık
            if (result.Succeeded)// bu işlemler başarılı olursa
            {
                return RedirectToAction("UserLogin", "Login"); //buraya gönder
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
