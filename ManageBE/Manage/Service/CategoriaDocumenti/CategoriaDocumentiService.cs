
using Manage.Models.Request;
using Manage.Repository.CategoriaDocumenti;

namespace Manage.Service.CategoriaDocumenti
{
    public class CategoriaDocumentiService : ICategoriaDocumentiService
    {
        private readonly ICategoriaDocumentiRepository _repository;

        public CategoriaDocumentiService(ICategoriaDocumentiRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Manage.Models.CategoriaDocumenti>> GetAllCategoriaDocumentiAsync(string idUtente)
        {
            return await _repository.GetAllAsync(idUtente);
        }

        public async Task<Manage.Models.CategoriaDocumenti> GetCategoriaDocumentiByIdAsync(int id, string idUtente)
        {
            return await _repository.GetByIdAsync(id, idUtente);
        }

        public async Task<Manage.Models.CategoriaDocumenti> CreateCategoriaDocumentiAsync(Manage.Models.Request.CategoriaDocumentiRequest categoriaDocumentiRequest, string idUtente)
        {
            return await _repository.AddAsync(categoriaDocumentiRequest, idUtente);
        }

        public async Task<Manage.Models.CategoriaDocumenti> UpdateCategoriaDocumentiAsync(Manage.Models.CategoriaDocumenti categoriaDocumenti)
        {
            return await _repository.UpdateAsync(categoriaDocumenti);
        }
        public async Task DeleteCategoriaDocumentiAsync(int id, string idUtente)
        {
            await _repository.DeleteAsync(id, idUtente);
        }
    }
}
