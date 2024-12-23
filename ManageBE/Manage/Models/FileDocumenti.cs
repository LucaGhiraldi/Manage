namespace Manage.Models
{
    public class FileDocumenti
    {
        public int Id { get; set; }
        public string NomeFile { get; set; }
        public string EstensioneFile { get; set; }
        public string PercorsoFile { get; set; }
        public DateTime DataInserimentoFile { get; set; } = DateTime.Now;

        // Foreign Key per la relazione con Documenti
        public int DocumentiId { get; set; }
        public Documenti Documento { get; set; }
    }
}
