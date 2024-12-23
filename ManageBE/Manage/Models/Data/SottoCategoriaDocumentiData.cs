namespace Manage.Models.Data
{
    public class SottoCategoriaDocumentiData
    {
        public int Id { get; set; }
        public string NomeSottoCategoria { get; set; }
        public string DescrizioneSottoCategoria { get; set; }
        public DateTime DataInserimentoSottoCategoria { get; set; } = DateTime.Now;
    }
}
