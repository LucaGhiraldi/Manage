namespace Manage.Models.Request
{
    public class DocumentiRequest
    {
        public string TitoloDocumento { get; set; }
        public string DescrizioneDocumento { get; set; }
        public DateTime DataCreazioneDocumento { get; set; }

        // ID della utente
        public string UtenteId { get; set; }

        // ID della categoria selezionata dall'utente
        //public int CategoriaDocumentiId { get; set; }
        public int SottoCategoriaDocumentiId { get; set; }


        // Lista dei file associati al documento (nome e percorso)
        public List<FileDocumentiRequest> FileDocumenti { get; set; }
    }

    public class DocumentiRequestPartial
    {
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazioneDocumento { get; set; }

        // ID della categoria selezionata dall'utente
        //public int CategoriaDocumentiId { get; set; }
        // ID della sotto categoria selezionata dall'utente
        public int SottoCategoriaDocumentiId { get; set; }
    }

    public class DocumentiRequestFilter
    {
        public string? Titolo { get; set; }
        public string? Descrizione { get; set; }
        public DateTime? DataCreazioneDocumento { get; set; }
        public DateTime? DataInserimentoDocumento { get; set; }
        //public int? CategoriaDocumentiId { get; set; }
        public int? SottoCategoriaId { get; set; }
    }
}
