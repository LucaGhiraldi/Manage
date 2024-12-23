using Microsoft.AspNetCore.Mvc;

namespace Manage.Service.FileDocumenti
{
    public interface IFileDocumentiService
    {
        Task<IActionResult> GetDownloadFileDocumentiByIdAsync(int id);
        Task<IActionResult> GetDownaloadFileDocumentiByListIdAsync(List<int> ids);
        Task<List<Manage.Models.FileDocumenti>> GetFileDocumentiByListIdAsync(List<int> id);

    }
}
