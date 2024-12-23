namespace Manage.Models
{
    public class CategoriaDocumenti
    {
        public int Id { get; set; }
        public string NomeCategoria { get; set; }
        public string DescrizioneCategoria { get; set; }
        public DateTime DataInserimentoCategoria { get; set; } = DateTime.Now;
        // Indica se la categoria è predefinita
        public bool IsPredefinita { get; set; }


        // Relazione uno-a-molti con Documenti
        public ICollection<Documenti> Documenti { get; set; }

        // Relazione uno-a-molti con SottoCategoriaDocumenti
        public ICollection<SottoCategoriaDocumenti> SottoCategorie { get; set; }

        // FK verso Utente
        public string? UtenteId { get; set; }
        public Utente Utente { get; set; }

    }
}
