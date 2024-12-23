using Manage.Models.Data;
using Manage.Models.Request;
using Manage.Repository.Documenti;

namespace Manage.Service.Documenti
{
    public class DocumentiService : IDocumentiService
    {
        private readonly IDocumentiRepository _repository;

        public DocumentiService(IDocumentiRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Manage.Models.Documenti>> GetAllDocumentiAsync(DocumentiRequestFilter documentiRequestFilter, string idUtente)
        {
            return await _repository.GetAllAsync(documentiRequestFilter, idUtente);
        }

        public async Task<Manage.Models.Documenti> GetDocumentiByIdAsync(int id, string idUtente)
        {
            return await _repository.GetByIdAsync(id, idUtente);
        }

        public async Task<Manage.Models.Documenti> CreateDocumentiAsync(Manage.Models.Request.DocumentiRequest documentiRequest, IFormFile[] newFiles)
        {
            return await _repository.AddAsync(documentiRequest, newFiles);
        }

        public async Task<Manage.Models.Documenti> UpdateDocumentiAsync(Manage.Models.Documenti documenti, List<int> idFilesDocumenti, IFormFile[] newFiles)
        {
            return await _repository.UpdateAsync(documenti, idFilesDocumenti, newFiles);
        }

        public async Task<Manage.Models.Documenti> UpdateDocumentiAsync(DocumentiDataPartial documenti, List<int> idFilesDocumenti, IFormFile[] newFiles, string utenteId)
        {
            return await _repository.UpdateAsync(documenti, idFilesDocumenti, newFiles, utenteId);
        }

        public async Task DeleteDocumentiAsync(int id, string idUtente)
        {
            await _repository.DeleteAsync(id, idUtente);
        }
    }
}
