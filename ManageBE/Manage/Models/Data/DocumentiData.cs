namespace Manage.Models.Data
{
    public class DocumentiData
    {
        public int Id { get; set; }
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazioneDocumento { get; set; }
        public DateTime DataInserimentoDocumento { get; set; } = DateTime.Now;

        public string UtenteId { get; set; }

        public int CategoriaDocumentiId { get; set; }
        public string NomeCategoriaDocumenti { get; set; }

        public int SottoCategoriaDocumentiId { get; set; }
        public string NomeSottoCategoriaDocumenti { get; set; }

        public List<int> idFiles { get; set; }
        public List<string> NomiFiles { get; set; }
    }

    public class DocumentiDataPartial
    {
        public int Id { get; set; }
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataCreazioneDocumento { get; set; }

        //public int CategoriaDocumentiId { get; set; }
        public int SottoCategoriaDocumentiId { get; set; }

        public List<int> idFiles { get; set; } = new List<int>();
    }
}
