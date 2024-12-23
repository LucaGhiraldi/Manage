using Microsoft.AspNetCore.Mvc;

namespace Manage.Repository.FileDocumenti
{
    public interface IFileDocumentiRepository
    {
        Task<IActionResult> GetDownloadByIdAsync(int id);
        Task<IActionResult> GetDownloadByListIdAsync(List<int> ids);
        
        Task<List<Manage.Models.FileDocumenti>> GetByListIdAsync(List<int> id);
    }
}
