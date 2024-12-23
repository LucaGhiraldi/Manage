namespace Manage.Models
{
    public class Documenti
    {
        public int Id { get; set; }
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazioneDocumento { get; set; }
        public DateTime DataInserimentoDocumento { get; set; } = DateTime.Now;

        // Foreign Key per la relazione con Utente
        public string UtenteId { get; set; }
        public Utente Utente { get; set; }

        // Foreign Key per la relazione con CategoriaDocumenti
        public int CategoriaDocumentiId { get; set; }
        public CategoriaDocumenti Categoria { get; set; }

        // FK verso SottoCategoriaDocumenti (può essere nullable)
        public int? SottoCategoriaDocumentiId { get; set; }
        public SottoCategoriaDocumenti? SottoCategoria { get; set; }

        // Relazione uno-a-molti con FileDocumenti
        public ICollection<FileDocumenti> FileDocumenti { get; set; }
    }
}
