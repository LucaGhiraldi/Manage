using Manage.Models.Data;
using Manage.Models.Request;

namespace Manage.Service.Documenti
{
    public interface IDocumentiService
    {
        Task<IEnumerable<Manage.Models.Documenti>> GetAllDocumentiAsync(DocumentiRequestFilter documentiRequestFilter, string idUtente);
        Task<Manage.Models.Documenti> GetDocumentiByIdAsync(int id, string idUtente);
        Task<Manage.Models.Documenti> CreateDocumentiAsync(Manage.Models.Request.DocumentiRequest documentiRequest, IFormFile[] newFiles);
        Task<Manage.Models.Documenti> UpdateDocumentiAsync(Manage.Models.Documenti documenti, List<int> idFilesDocumenti, IFormFile[] newFiles);
        Task<Manage.Models.Documenti> UpdateDocumentiAsync(DocumentiDataPartial documenti, List<int> idFilesDocumenti, IFormFile[] newFiles, string utenteId);

        Task DeleteDocumentiAsync(int id, string idUtente);
    }
}
