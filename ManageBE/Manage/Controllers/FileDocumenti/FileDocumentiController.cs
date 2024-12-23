using Manage.Models.Data;
using Manage.Models.Request;
using Manage.Service.FileDocumenti;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Manage.Controllers.FileDocumenti
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FileDocumentiController : ControllerBase
    {
        private readonly IFileDocumentiService _fileDocumentiService;

        public FileDocumentiController(IFileDocumentiService fileDocumentiService)
        {
            _fileDocumentiService = fileDocumentiService;
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> GetDownloadById(int id)
        {
            try
            {
                var fileResult = await _fileDocumentiService.GetDownloadFileDocumentiByIdAsync(id);

                return fileResult;
            }
            catch (Exception ex) 
            {
                return NotFound("File non trovato sul percorso specificato.");
            }
        }
        
        [HttpGet("DownloadAll")]
        public async Task<IActionResult> GetDownloadByListId([FromQuery] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("È necessario fornire almeno un ID.");
            }

            try
            {
                var fileResult = await _fileDocumentiService.GetDownaloadFileDocumentiByListIdAsync(ids);

                return fileResult;
            }
            catch (Exception ex)
            {
                // Qui puoi loggare l'eccezione per scopi di debug, se necessario
                return NotFound("Nessun file trovato per gli ID specificati.");
            }
        }

        [HttpPost("GetByListId")]
        public async Task<IActionResult> GetByListId([FromBody] List<int> idFiles)
        {
            // Recupera i documenti per l'utente specificato
            var fileDocumenti = await _fileDocumentiService.GetFileDocumentiByListIdAsync(idFiles);

            List<FileDocumentiData> listaFileDocumenti = new();

            foreach (var file in fileDocumenti)
            {
                FileDocumentiData fileDocumentiData = new();
                fileDocumentiData.Id = file.Id;
                fileDocumentiData.NomeFile = file.NomeFile;
                fileDocumentiData.EstensioneFile = file.EstensioneFile;
                fileDocumentiData.PercorsoFile = file.PercorsoFile;
                fileDocumentiData.DataInserimentoFile = file.DataInserimentoFile;
                fileDocumentiData.DocumentiId = file.DocumentiId;

                listaFileDocumenti.Add(fileDocumentiData);
            }

            return Ok(listaFileDocumenti);
        }

    }
}
