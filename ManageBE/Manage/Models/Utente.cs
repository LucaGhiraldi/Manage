using Manage.Models.NetWorth.Base;
using Microsoft.AspNetCore.Identity;

namespace Manage.Models
{
    public class Utente : IdentityUser
    {
        //public int Id { get; set; }
        //public string Nome { get; set; }
        //public string Cognome { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        public DateTime DataInserimentoUtente { get; set; } = DateTime.Now;

        // Relazione uno-a-molti con Documenti
        public ICollection<Documenti> Documenti { get; set; } = new List<Documenti>();

        // Relazione uno-a-molti con CategoriaDocumenti
        public ICollection<CategoriaDocumenti> CategorieDocumenti { get; set; } = new List<CategoriaDocumenti>();

        // Relazione uno-a-molti con Investimento
        public ICollection<InvestimentoBase> Investimenti { get; set; } = new List<InvestimentoBase>();
    }
}
