using Manage.Models.Data;
using Manage.Models.Request;

namespace Manage.Repository.Documenti
{
    public interface IDocumentiRepository
    {
        Task<IEnumerable<Manage.Models.Documenti>> GetAllAsync(DocumentiRequestFilter documentiRequestFilter, string idUtente);
        Task<Manage.Models.Documenti> GetByIdAsync(int id, string idUtente);
        Task<Manage.Models.Documenti> AddAsync(Manage.Models.Request.DocumentiRequest documentiRequest, IFormFile[] newFiles);
        Task<Manage.Models.Documenti> UpdateAsync(Manage.Models.Documenti documenti, List<int> idFilesDocumenti, IFormFile[] newFiles);
        Task<Manage.Models.Documenti> UpdateAsync(DocumentiDataPartial documenti, List<int> idFilesDocumenti, IFormFile[] newFiles, string utenteId);

        Task DeleteAsync(int id, string idUtente);
    }
}
