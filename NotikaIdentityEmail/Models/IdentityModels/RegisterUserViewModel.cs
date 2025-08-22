namespace NotikaIdentityEmail.Models.IdentityModels
{
    public class RegisterUserViewModel //ihtiyacımız olan sınıfları çağırmak için bir nevi dto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
