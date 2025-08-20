namespace NotikaIdentityEmail.Models
{
    public class RegisterUserViewModel //ihtiyacımız olan sınıfları çağırmak için bir nevi dto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Pasword { get; set; }
    }
}
