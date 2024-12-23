using Manage.Repository.Documenti;
using Manage.Repository.FileDocumenti;
using Microsoft.AspNetCore.Mvc;

namespace Manage.Service.FileDocumenti
{
    public class FileDocumentiService : IFileDocumentiService
    {
        private readonly IFileDocumentiRepository _repository;

        public FileDocumentiService(IFileDocumentiRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> GetDownloadFileDocumentiByIdAsync(int id)
        {
            return await _repository.GetDownloadByIdAsync(id);
        }

        public async Task<IActionResult> GetDownaloadFileDocumentiByListIdAsync(List<int> ids)
        {
            return await _repository.GetDownloadByListIdAsync(ids);
        }

        public async Task<List<Manage.Models.FileDocumenti>> GetFileDocumentiByListIdAsync(List<int> id)
        {
            return await _repository.GetByListIdAsync(id);
        }
    }
}
