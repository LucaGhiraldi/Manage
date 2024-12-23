namespace Manage.Models.Data
{
    public class CategoriaDocumentiData
    {
        public int Id { get; set; }
        public string NomeCategoria { get; set; }
        public string DescrizioneCategoria { get; set; }
        public DateTime DataInserimentoCategoria { get; set; } = DateTime.Now;
        public bool IsPredefinita { get; set; }
        public string UtenteId { get; set; }

        // Aggiungi la lista di sotto-categorie
        public List<SottoCategoriaDocumentiData> SottoCategorie { get; set; } = new List<SottoCategoriaDocumentiData>();
    }

    public class CategoriaDocumentiDataPartial
    {
        public int Id { get; set; }
        public string NomeCategoria { get; set; }
        public string DescrizioneCategoria { get; set; }
        public DateTime DataInserimentoCategoria { get; set; } = DateTime.Now;
        // Aggiungi la lista di sotto-categorie
        public List<SottoCategoriaDocumentiData> SottoCategorie { get; set; } = new List<SottoCategoriaDocumentiData>();
    }
}
