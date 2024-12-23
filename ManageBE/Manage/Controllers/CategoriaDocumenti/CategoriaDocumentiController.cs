using Manage.Models;
using Manage.Models.Data;
using Manage.Models.Request;
using Manage.Service.CategoriaDocumenti;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Manage.Controllers.CategoriaDocumenti
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriaDocumentiController : ControllerBase
    {
        private readonly ICategoriaDocumentiService _categoriaDocumentiService;
        private readonly IWebHostEnvironment _env;  // Aggiungi IWebHostEnvironment

        public CategoriaDocumentiController(ICategoriaDocumentiService categoriaDocumentiService, IWebHostEnvironment env)
        {
            _categoriaDocumentiService = categoriaDocumentiService;
            _env = env;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Recupera i documenti per l'utente specificato
                var categoriaDocumenti = await _categoriaDocumentiService.GetAllCategoriaDocumentiAsync(userId);

                // Se non ci sono documenti, restituisci una risposta vuota o NotFound
                if (categoriaDocumenti == null || !categoriaDocumenti.Any())
                {
                    return NotFound($"Nessun documento trovato per l'utente con id {userId}");
                }

                List<CategoriaDocumentiData> listaCategoriaDocumenti = new();
                listaCategoriaDocumenti = categoriaDocumenti.Select(c => new CategoriaDocumentiData
                {
                    Id = c.Id,
                    NomeCategoria = c.NomeCategoria,
                    DescrizioneCategoria = c.DescrizioneCategoria,
                    DataInserimentoCategoria = c.DataInserimentoCategoria,
                    IsPredefinita = c.IsPredefinita,
                    UtenteId = c.UtenteId,
                    // Mappa le sotto-categorie
                    SottoCategorie = c.SottoCategorie.Select(s => new SottoCategoriaDocumentiData
                    {
                        Id = s.Id,
                        NomeSottoCategoria = s.NomeSottoCategoria,
                        DescrizioneSottoCategoria = s.DescrizioneSottoCategoria,
                        DataInserimentoSottoCategoria = s.DataInserimentoSottoCategoria
                    }).ToList()
                }).ToList();

                //foreach (var categoriaDocumento in categoriaDocumenti)
                //{
                //    CategoriaDocumentiData categoriaDocumentiData = new();
                //    categoriaDocumentiData.Id = categoriaDocumento.Id;
                //    categoriaDocumentiData.NomeCategoria = categoriaDocumento.NomeCategoria;
                //    categoriaDocumentiData.DescrizioneCategoria = categoriaDocumento.DescrizioneCategoria;
                //    categoriaDocumentiData.DataInserimentoCategoria = categoriaDocumento.DataInserimentoCategoria;
                //    categoriaDocumentiData.IsPredefinita = categoriaDocumento.IsPredefinita;
                //    categoriaDocumentiData.UtenteId = categoriaDocumento.UtenteId;

                //    listaCategoriaDocumenti.Add(categoriaDocumentiData);
                //}

                return Ok(listaCategoriaDocumenti);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                var categoriaDocumenti = await _categoriaDocumentiService.GetCategoriaDocumentiByIdAsync(id, userId);
                if (categoriaDocumenti == null)
                    return NotFound();

                CategoriaDocumentiData categoriaDocumentiData = new();
                categoriaDocumentiData = new CategoriaDocumentiData
                {
                    Id = categoriaDocumenti.Id,
                    NomeCategoria = categoriaDocumenti.NomeCategoria,
                    DescrizioneCategoria = categoriaDocumenti.DescrizioneCategoria,
                    DataInserimentoCategoria = categoriaDocumenti.DataInserimentoCategoria,
                    IsPredefinita = categoriaDocumenti.IsPredefinita,
                    UtenteId = categoriaDocumenti.UtenteId,
                    // Mappa le sotto-categorie
                    SottoCategorie = categoriaDocumenti.SottoCategorie.Select(s => new SottoCategoriaDocumentiData
                    {
                        Id = s.Id,
                        NomeSottoCategoria = s.NomeSottoCategoria,
                        DescrizioneSottoCategoria = s.DescrizioneSottoCategoria,
                        DataInserimentoSottoCategoria = s.DataInserimentoSottoCategoria
                    }).ToList()
                };

                //categoriaDocumentiData.Id = categoriaDocumenti.Id;
                //categoriaDocumentiData.NomeCategoria = categoriaDocumenti.NomeCategoria;
                //categoriaDocumentiData.DescrizioneCategoria = categoriaDocumenti.DescrizioneCategoria;
                //categoriaDocumentiData.DataInserimentoCategoria = categoriaDocumenti.DataInserimentoCategoria;
                //categoriaDocumentiData.IsPredefinita = categoriaDocumenti.IsPredefinita;
                //categoriaDocumentiData.UtenteId = categoriaDocumenti.UtenteId;

                return Ok(categoriaDocumentiData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Manage.Models.Request.CategoriaDocumentiRequest categoriaDocumentiRequest)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Verifica dati principali
                if (String.IsNullOrWhiteSpace(categoriaDocumentiRequest.NomeCategoria) ||
                    String.IsNullOrWhiteSpace(categoriaDocumentiRequest.DescrizioneCategoria))
                {
                    return BadRequest("Alcune informazioni non sono state inserite");
                }

                // La verifica successiva è effettuata all'interno del repository
                var newCategoriaDocumenti = await _categoriaDocumentiService.CreateCategoriaDocumentiAsync(categoriaDocumentiRequest, userId);

                CategoriaDocumentiData categoriaDocumentiData = new();
                categoriaDocumentiData = new CategoriaDocumentiData
                {
                    Id = newCategoriaDocumenti.Id,
                    NomeCategoria = newCategoriaDocumenti.NomeCategoria,
                    DescrizioneCategoria = newCategoriaDocumenti.DescrizioneCategoria,
                    DataInserimentoCategoria = newCategoriaDocumenti.DataInserimentoCategoria,
                    IsPredefinita = newCategoriaDocumenti.IsPredefinita,
                    UtenteId = newCategoriaDocumenti.UtenteId,
                    // Mappa le sotto-categorie
                    SottoCategorie = newCategoriaDocumenti.SottoCategorie.Select(s => new SottoCategoriaDocumentiData
                    {
                        Id = s.Id,
                        NomeSottoCategoria = s.NomeSottoCategoria,
                        DescrizioneSottoCategoria = s.DescrizioneSottoCategoria,
                        DataInserimentoSottoCategoria = s.DataInserimentoSottoCategoria
                    }).ToList()
                };

                //categoriaDocumentiData.Id = newCategoriaDocumenti.Id;
                //categoriaDocumentiData.NomeCategoria = newCategoriaDocumenti.NomeCategoria;
                //categoriaDocumentiData.DescrizioneCategoria = newCategoriaDocumenti.DescrizioneCategoria;
                //categoriaDocumentiData.DataInserimentoCategoria = newCategoriaDocumenti.DataInserimentoCategoria;
                //categoriaDocumentiData.IsPredefinita = newCategoriaDocumenti.IsPredefinita;
                //categoriaDocumentiData.UtenteId = newCategoriaDocumenti.UtenteId;

                // Restituisco l'informazione inserita
                return CreatedAtAction(nameof(GetById), new { id = categoriaDocumentiData.Id }, categoriaDocumentiData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CategoriaDocumentiDataPartial categoriaDocumentiRequest)
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized();
                }

                // Verifica dati principali
                if (String.IsNullOrWhiteSpace(categoriaDocumentiRequest.NomeCategoria) ||
                    String.IsNullOrWhiteSpace(categoriaDocumentiRequest.DescrizioneCategoria))
                {
                    return BadRequest("Alcune informazioni non sono state inserite");
                }

                // Prepara l'oggetto categoria da aggiornare con le sotto-categorie
                var categoriaDocumenti = new Manage.Models.CategoriaDocumenti
                {
                    Id = categoriaDocumentiRequest.Id,
                    NomeCategoria = categoriaDocumentiRequest.NomeCategoria,
                    DescrizioneCategoria = categoriaDocumentiRequest.DescrizioneCategoria,
                    DataInserimentoCategoria = DateTime.Now,
                    IsPredefinita = false,
                    UtenteId = userId,
                    SottoCategorie = categoriaDocumentiRequest.SottoCategorie
                        .Select(sc => new Manage.Models.SottoCategoriaDocumenti
                        {
                            Id = sc.Id,
                            NomeSottoCategoria = sc.NomeSottoCategoria,
                            DescrizioneSottoCategoria = sc.DescrizioneSottoCategoria,
                            DataInserimentoSottoCategoria = sc.DataInserimentoSottoCategoria
                        })
                        .ToList()
                };

                List<int> idSottoCategorie = categoriaDocumentiRequest.SottoCategorie.Select(sc => sc.Id).ToList() ?? new List<int>();

                var updatedCategoriaDocumenti = await _categoriaDocumentiService.UpdateCategoriaDocumentiAsync(categoriaDocumenti);

                // Prepara la risposta con le sotto-categorie aggiornate
                var categoriaDocumentiData = new CategoriaDocumentiData
                {
                    Id = updatedCategoriaDocumenti.Id,
                    NomeCategoria = updatedCategoriaDocumenti.NomeCategoria,
                    DescrizioneCategoria = updatedCategoriaDocumenti.DescrizioneCategoria,
                    DataInserimentoCategoria = updatedCategoriaDocumenti.DataInserimentoCategoria,
                    IsPredefinita = updatedCategoriaDocumenti.IsPredefinita,
                    UtenteId = updatedCategoriaDocumenti.UtenteId,
                    SottoCategorie = updatedCategoriaDocumenti.SottoCategorie
                        .Select(sc => new SottoCategoriaDocumentiData
                        {
                            Id = sc.Id,
                            NomeSottoCategoria = sc.NomeSottoCategoria,
                            DescrizioneSottoCategoria = sc.DescrizioneSottoCategoria,
                            DataInserimentoSottoCategoria = sc.DataInserimentoSottoCategoria
                        })
                        .ToList()
                };

                return Ok(categoriaDocumentiData);
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

                await _categoriaDocumentiService.DeleteCategoriaDocumentiAsync(id, userId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
