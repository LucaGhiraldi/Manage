namespace Manage.Repository.CategoriaDocumenti
{
    public interface ICategoriaDocumentiRepository
    {
        Task<IEnumerable<Manage.Models.CategoriaDocumenti>> GetAllAsync(string idUtente);
        Task<Manage.Models.CategoriaDocumenti> GetByIdAsync(int id, string idUtente);
        Task<Manage.Models.CategoriaDocumenti> AddAsync(Manage.Models.Request.CategoriaDocumentiRequest categoriaDocumentiRequest, string idUtente);
        Task<Manage.Models.CategoriaDocumenti> UpdateAsync(Manage.Models.CategoriaDocumenti categoriaDocumenti);
        Task DeleteAsync(int id, string idUtente);

    }
}
