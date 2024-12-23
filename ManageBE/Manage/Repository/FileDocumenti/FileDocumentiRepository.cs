using Manage.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Manage.Repository.FileDocumenti
{
    public class FileDocumentiRepository : IFileDocumentiRepository
    {
        private readonly ApplicationDbContext _context;

        public FileDocumentiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetDownloadByIdAsync(int id)
        {
            var fileDocumenti = await _context.FileDocumenti
                                            .FirstOrDefaultAsync(d => d.Id == id);

            if (fileDocumenti == null)
            {
                throw new Exception($"File documento con id {id} non trovato.");
            }

            // Costruisci il percorso fisico completo (supponiamo che PercorsoFisico memorizzi il percorso relativo)
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileDocumenti.PercorsoFile);

            // Leggi il file dal percorso fisico
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = fileDocumenti.NomeFile + fileDocumenti.EstensioneFile,
            };
        }

        public async Task<IActionResult> GetDownloadByListIdAsync(List<int> ids)
        {
            // Recupera i file documenti corrispondenti agli ID forniti
            var fileDocumenti = await _context.FileDocumenti
                                                .Where(d => ids.Contains(d.Id))
                                                .ToListAsync();

            // Controlla se ci sono file documenti trovati
            if (fileDocumenti.Count == 0)
            {
                throw new Exception("Nessun file documento trovato per gli ID forniti.");
            }

            // Creare un archivio zip in memoria
            var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var fileDocument in fileDocumenti)
                {
                    // Costruisci il percorso fisico completo
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileDocument.PercorsoFile);

                    // Aggiungi il file all'archivio zip
                    var zipEntry = zipArchive.CreateEntry(fileDocument.NomeFile + fileDocument.EstensioneFile);
                    using (var entryStream = zipEntry.Open())
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        await fileStream.CopyToAsync(entryStream);
                    }
                }
            }

            // Ripristina la posizione dello stream in memoria per la lettura
            memoryStream.Position = 0;

            // Restituisci il file zip come risultato
            return new FileStreamResult(memoryStream, "application/zip")
            {
                FileDownloadName = $"{DateTime.Now}_FileDocumenti.zip",
            };
        }

        public async Task<List<Manage.Models.FileDocumenti>> GetByListIdAsync(List<int> id)
        {
            // Ottieni tutti i file i cui ID sono contenuti nella lista `ids`
            var fileDocumenti = await _context.FileDocumenti
                                              .Where(d => id.Contains(d.Id))
                                              .ToListAsync();

            if (fileDocumenti == null)
            {
                throw new Exception($"File documento con id {id} non trovato.");
            }

            return fileDocumenti;
        }
    }
}
