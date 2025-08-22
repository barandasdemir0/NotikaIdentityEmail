namespace NotikaIdentityEmail.Models.JwtModels
{
    public class JwtSettingsModel
    {
        public string Key { get; set; } //jsonda verilen değer
        public string Issuer { get; set; } //jsonda verilen değer
        public string Audience { get; set; } //jsonda verilen değer
        public int ExpireMinutes { get; set; } //jsonda verilen değer

    }
}
