using Manage.Data;
using Manage.Models;
using Manage.Models.Data;
using Manage.Models.Request;
using Manage.Service.Documenti;
using Manage.Service.FileDocumenti;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Manage.Controllers.Documenti
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentiController : ControllerBase
    {
        private readonly IDocumentiService _documentiService;
        private readonly IFileDocumentiService _fileDocumentiService;
        private readonly IWebHostEnvironment _env;  // Aggiungi IWebHostEnvironment

        public DocumentiController(IDocumentiService documentiService, IWebHostEnvironment env, IFileDocumentiService fileDocumentiService)
        {
            _documentiService = documentiService;
            _env = env;
            _fileDocumentiService = fileDocumentiService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] DocumentiRequestFilter documentiRequestFilter)
        {

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Recupera i documenti per l'utente specificato
            var documenti = await _documentiService.GetAllDocumentiAsync(documentiRequestFilter, userId);

            List<DocumentiData> listaDocumenti = new();

            foreach(var documento in documenti)
            {
                DocumentiData documentiData = new();
                documentiData.Id = documento.Id;
                documentiData.Titolo = documento.Titolo;
                documentiData.Descrizione = documento.Descrizione;
                documentiData.DataCreazioneDocumento = documento.DataCreazioneDocumento;
                documentiData.DataInserimentoDocumento = documento.DataInserimentoDocumento;
                documentiData.UtenteId = documento.UtenteId;
                documentiData.CategoriaDocumentiId = documento.CategoriaDocumentiId;
                documentiData.NomeCategoriaDocumenti = documento.Categoria.NomeCategoria;

                documentiData.SottoCategoriaDocumentiId = (int)documento.SottoCategoriaDocumentiId;
                documentiData.NomeSottoCategoriaDocumenti = documento.SottoCategoria.NomeSottoCategoria;

                documentiData.idFiles = documento.FileDocumenti.Select(f => f.Id).ToList();
                documentiData.NomiFiles = documento.FileDocumenti.Select(f => f.NomeFile + f.EstensioneFile).ToList();

                listaDocumenti.Add(documentiData);
            }

            return Ok(listaDocumenti);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var documenti = await _documentiService.GetDocumentiByIdAsync(id, userId);

            DocumentiData documentiData = new();
            documentiData.Id = documenti.Id;
            documentiData.Titolo = documenti.Titolo;
            documentiData.Descrizione = documenti.Descrizione;
            documentiData.DataCreazioneDocumento = documenti.DataCreazioneDocumento;
            documentiData.DataInserimentoDocumento = documenti.DataInserimentoDocumento;
            documentiData.UtenteId = documenti.UtenteId;
            documentiData.CategoriaDocumentiId = documenti.CategoriaDocumentiId;
            documentiData.NomeCategoriaDocumenti = documenti.Categoria.NomeCategoria;

            documentiData.SottoCategoriaDocumentiId = (int)documenti.SottoCategoriaDocumentiId;
            documentiData.NomeSottoCategoriaDocumenti = documenti.SottoCategoria.NomeSottoCategoria;

            documentiData.idFiles = documenti.FileDocumenti.Select(f => f.Id).ToList();
            documentiData.NomiFiles = documenti.FileDocumenti.Select(f => f.NomeFile).ToList();

            return Ok(documentiData);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Manage.Models.Request.DocumentiRequestPartial documentiRequest, IFormFile[] files)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Verifica dati principali
                if (String.IsNullOrWhiteSpace(documentiRequest.Titolo)          ||
                    String.IsNullOrWhiteSpace(documentiRequest.Descrizione)     ||
                    documentiRequest.SottoCategoriaDocumentiId == 0                  ||
                    documentiRequest.DataCreazioneDocumento == null)
                {
                    return BadRequest("Alcune informazioni non sono state inserite");
                }

                Manage.Models.Request.DocumentiRequest documentiRequestFinal = new();
                documentiRequestFinal.TitoloDocumento = documentiRequest.Titolo;
                documentiRequestFinal.DescrizioneDocumento = documentiRequest.Descrizione;
                documentiRequestFinal.DataCreazioneDocumento = documentiRequest.DataCreazioneDocumento.ToLocalTime();
                documentiRequestFinal.UtenteId = userId;
                //documentiRequestFinal.CategoriaDocumentiId = documentiRequest.CategoriaDocumentiId;

                documentiRequestFinal.SottoCategoriaDocumentiId = documentiRequest.SottoCategoriaDocumentiId;
                //documentiRequestFinal.FileDocumenti = savedFiles;

                // La verifica successiva è effettuata all'interno del repository
                var newDocumenti = await _documentiService.CreateDocumentiAsync(documentiRequestFinal, files);

                DocumentiData documentiData = new();
                documentiData.Id = newDocumenti.Id;
                documentiData.Titolo = newDocumenti.Titolo;
                documentiData.Descrizione = newDocumenti.Descrizione;
                documentiData.DataCreazioneDocumento = newDocumenti.DataCreazioneDocumento;
                documentiData.DataInserimentoDocumento = newDocumenti.DataInserimentoDocumento;
                documentiData.UtenteId = newDocumenti.UtenteId;
                documentiData.CategoriaDocumentiId = newDocumenti.CategoriaDocumentiId;
                documentiData.NomeCategoriaDocumenti = newDocumenti.Categoria.NomeCategoria;

                documentiData.SottoCategoriaDocumentiId = (int)newDocumenti.SottoCategoriaDocumentiId;
                documentiData.NomeSottoCategoriaDocumenti = newDocumenti.SottoCategoria.NomeSottoCategoria;
                //documentiData.idFiles = newDocumenti.FileDocumenti.Select(f => f.Id).ToList();
                //documentiData.NomiFiles = newDocumenti.FileDocumenti.Select(f => f.NomeFile).ToList();

                documentiData.idFiles = new();

                if (newDocumenti.FileDocumenti != null && newDocumenti.FileDocumenti.Count > 0)
                {
                    foreach (var fileDocumento in newDocumenti.FileDocumenti)
                    {
                        documentiData.idFiles.Add(fileDocumento.Id);
                    }
                }

                // Restituisco l'informazione inserita
                return CreatedAtAction(nameof(GetById), new { id = documentiData.Id }, documentiData);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] DocumentiDataPartial documenti, IFormFile[] files)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Verifica dati principali
                if (String.IsNullOrWhiteSpace(documenti.Titolo)                 ||
                    String.IsNullOrWhiteSpace(documenti.Descrizione)            ||
                    //documenti.CategoriaDocumentiId == 0                         ||
                    documenti.SottoCategoriaDocumentiId == 0                    ||
                    documenti.Id == 0                                           ||
                    documenti.DataCreazioneDocumento == null)
                {
                    return BadRequest("Alcune informazioni non sono state inserite");
                }

                List<int> idFilesDocumenti = documenti.idFiles ?? new List<int>();

                // Chiamata Update con request
                var updatedDocumenti = await _documentiService.UpdateDocumentiAsync(documenti, idFilesDocumenti, files, userId);

                DocumentiData documentiData = new();
                documentiData.Id = updatedDocumenti.Id;
                documentiData.Titolo = updatedDocumenti.Titolo;
                documentiData.Descrizione = updatedDocumenti.Descrizione;
                documentiData.DataCreazioneDocumento = updatedDocumenti.DataCreazioneDocumento;
                documentiData.DataInserimentoDocumento = updatedDocumenti.DataInserimentoDocumento;
                documentiData.UtenteId = updatedDocumenti.UtenteId;
                documentiData.CategoriaDocumentiId = updatedDocumenti.CategoriaDocumentiId;
                documentiData.NomeCategoriaDocumenti = updatedDocumenti.Categoria.NomeCategoria;
                documentiData.SottoCategoriaDocumentiId = updatedDocumenti.CategoriaDocumentiId;
                documentiData.NomeSottoCategoriaDocumenti = updatedDocumenti.SottoCategoria.NomeSottoCategoria;
                documentiData.idFiles = updatedDocumenti.FileDocumenti?.Select(file => file.Id).ToList() ?? new List<int>();

                //// Prepara l'oggetto documento da aggiornare
                //Manage.Models.Documenti documentiFull = new()
                //{
                //    Id = documenti.Id,
                //    Titolo = documenti.Titolo,
                //    Descrizione = documenti.Descrizione,
                //    DataCreazioneDocumento = documenti.DataCreazioneDocumento.ToLocalTime(),
                //    DataInserimentoDocumento = DateTime.Now.ToLocalTime(),
                //    UtenteId = userId,
                //    CategoriaDocumentiId = documenti.CategoriaDocumentiId,
                //    SottoCategoriaDocumentiId = documenti.SottoCategoriaDocumentiId,
                //};

                //List<int> idFilesDocumenti = documenti.idFiles ?? new List<int>();

                //var updatedDocumenti = await _documentiService.UpdateDocumentiAsync(documentiFull, idFilesDocumenti, files);

                //DocumentiData documentiData = new();
                //documentiData.Id = updatedDocumenti.Id;
                //documentiData.Titolo = updatedDocumenti.Titolo;
                //documentiData.Descrizione = updatedDocumenti.Descrizione;
                //documentiData.DataCreazioneDocumento = updatedDocumenti.DataCreazioneDocumento;
                //documentiData.DataInserimentoDocumento = updatedDocumenti.DataInserimentoDocumento;
                //documentiData.UtenteId = updatedDocumenti.UtenteId;
                //documentiData.CategoriaDocumentiId = updatedDocumenti.CategoriaDocumentiId;
                //documentiData.NomeCategoriaDocumenti = updatedDocumenti.Categoria.NomeCategoria;
                //documentiData.SottoCategoriaDocumentiId = updatedDocumenti.CategoriaDocumentiId;
                //documentiData.NomeSottoCategoriaDocumenti = updatedDocumenti.SottoCategoria.NomeSottoCategoria;
                //documentiData.idFiles = updatedDocumenti.FileDocumenti?.Select(file => file.Id).ToList() ?? new List<int>();

                return Ok(documentiData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                await _documentiService.DeleteDocumentiAsync(id, userId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
