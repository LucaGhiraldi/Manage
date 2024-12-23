namespace Manage.Models
{
    public class SottoCategoriaDocumenti
    {
        public int Id { get; set; }
        public string NomeSottoCategoria { get; set; }
        public string DescrizioneSottoCategoria { get; set; }
        public DateTime DataInserimentoSottoCategoria { get; set; } = DateTime.Now;

        // Relazione molti-a-uno con CategoriaDocumenti
        public int CategoriaDocumentiId { get; set; }
        public CategoriaDocumenti CategoriaDocumenti { get; set; }

        // Relazione uno-a-molti con Documenti
        public ICollection<Documenti> Documenti { get; set; }
    }
}
