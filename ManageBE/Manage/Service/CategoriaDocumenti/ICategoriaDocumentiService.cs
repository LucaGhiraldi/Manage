namespace Manage.Service.CategoriaDocumenti
{
    public interface ICategoriaDocumentiService
    {
        Task<IEnumerable<Manage.Models.CategoriaDocumenti>> GetAllCategoriaDocumentiAsync(string idUtente);
        Task<Manage.Models.CategoriaDocumenti> GetCategoriaDocumentiByIdAsync(int id, string idUtente);
        Task<Manage.Models.CategoriaDocumenti> CreateCategoriaDocumentiAsync(Manage.Models.Request.CategoriaDocumentiRequest categoriaDocumentiRequest, string idUtente);
        Task<Manage.Models.CategoriaDocumenti> UpdateCategoriaDocumentiAsync(Manage.Models.CategoriaDocumenti categoriaDocumenti);
        Task DeleteCategoriaDocumentiAsync(int id, string idUtente);
    }
}
