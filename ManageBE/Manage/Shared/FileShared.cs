using Manage.Models.Request;
using Manage.Service.Documenti;
using Manage.Service.FileDocumenti;
using System.Runtime.InteropServices;

namespace Manage.Shared
{
    public class FileShared
    {
        private readonly IWebHostEnvironment _env;  // Aggiungi IWebHostEnvironment

        public FileShared(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Metodo per salvare nuovi file
        public async Task<List<Manage.Models.FileDocumenti>> SaveFilesAsync(IFormFile[] files, string utenteId, int documentoId)
        {
            var savedFiles = new List<Manage.Models.FileDocumenti>();
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", utenteId, documentoId.ToString());

            // Assicurati che la cartella esista
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var uniqueFileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Aggiungi i dettagli del file da salvare nel DB
                    savedFiles.Add(new Manage.Models.FileDocumenti
                    {
                        NomeFile = Path.GetFileNameWithoutExtension(file.FileName),
                        EstensioneFile = Path.GetExtension(file.FileName),
                        PercorsoFile = Path.Combine("uploads", utenteId, documentoId.ToString(), uniqueFileName),
                        DataInserimentoFile = DateTime.Now
                    });
                }
            }

            return savedFiles;
        }

        // Metodo per cancellare i file esistenti
        public void DeleteFiles(ICollection<Manage.Models.FileDocumenti> files)
        {
            foreach (var file in files)
            {
                var filePath = Path.Combine(_env.WebRootPath, file.PercorsoFile);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

    }
}
