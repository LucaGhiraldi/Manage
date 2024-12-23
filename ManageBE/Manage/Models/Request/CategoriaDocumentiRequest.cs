namespace Manage.Models.Request
{
    public class CategoriaDocumentiRequest
    {
        public string NomeCategoria { get; set; }
        public string DescrizioneCategoria { get; set; }
        public DateTime DataInserimentoCategoria { get; set; } = DateTime.Now;
        //public bool IsPredefinita { get; set; }

        // Aggiungi una lista di sotto-categorie
        public List<SottoCategoriaRequest> SottoCategorie { get; set; }
    }

    public class CategoriaDocumentiRequestUpdate
    {
        public int Id { get; set; }
        public string NomeCategoria { get; set; }
        public string DescrizioneCategoria { get; set; }
        public DateTime DataInserimentoCategoria { get; set; } = DateTime.Now;
        public string UtenteId { get; set; }
    }

}
