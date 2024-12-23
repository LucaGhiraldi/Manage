namespace Manage.Models.Data
{
    public class LoginDataValue
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DataInserimentoUtente { get; set; } = DateTime.Now;
    }
}
