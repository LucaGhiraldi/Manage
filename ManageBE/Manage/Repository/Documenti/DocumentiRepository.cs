using Manage.Data;
using Manage.Models;
using Manage.Models.Data;
using Manage.Models.Request;
using Manage.Service.Documenti;
using Manage.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Manage.Repository.Documenti
{
    public class DocumentiRepository : IDocumentiRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly FileShared _fileShared;

        public DocumentiRepository(ApplicationDbContext context, FileShared fileShared)
        {
            _context = context;
            _fileShared = fileShared;
        }

        public async Task<IEnumerable<Manage.Models.Documenti>> GetAllAsync(DocumentiRequestFilter documentiRequestFilter, string idUtente)
        {
            var query = _context.Documenti.AsQueryable();

            // Filtra per UtenteId
            //if (!String.IsNullOrWhiteSpace(idUtente))
            //{
                query = query.Where(d => d.UtenteId == idUtente);
            //}

            // Filtra per Titolo
            if (!string.IsNullOrEmpty(documentiRequestFilter.Titolo))
            {
                query = query.Where(d => d.Titolo.Contains(documentiRequestFilter.Titolo));
            }

            // Filtra per Descrizione
            if (!string.IsNullOrEmpty(documentiRequestFilter.Descrizione))
            {
                query = query.Where(d => d.Descrizione.Contains(documentiRequestFilter.Descrizione));
            }

            // Filtra per DataCreazioneDocumento
            if (!string.IsNullOrEmpty(documentiRequestFilter.DataCreazioneDocumento.ToString()) && documentiRequestFilter.DataCreazioneDocumento.HasValue)
            {
                query = query.Where(d => d.DataCreazioneDocumento >= documentiRequestFilter.DataCreazioneDocumento);
            }

            // Filtra per DataInserimentoDocumento
            if (!string.IsNullOrEmpty(documentiRequestFilter.DataInserimentoDocumento.ToString()) && documentiRequestFilter.DataInserimentoDocumento.HasValue)
            {
                query = query.Where(d => d.DataInserimentoDocumento >= documentiRequestFilter.DataInserimentoDocumento);
            }

            // Filtra per CategoriaDocumentiId
            //if (!string.IsNullOrEmpty(documentiRequestFilter.CategoriaDocumentiId.ToString()) && documentiRequestFilter.CategoriaDocumentiId != 0)
            //{
            //    query = query.Where(d => d.CategoriaDocumentiId == documentiRequestFilter.CategoriaDocumentiId);
            //}

            // Filtra per SottoCategoriaDocumentiId
            if (!string.IsNullOrEmpty(documentiRequestFilter.SottoCategoriaId.ToString()) && documentiRequestFilter.SottoCategoriaId != 0)
            {
                query = query.Where(d => d.SottoCategoriaDocumentiId == documentiRequestFilter.SottoCategoriaId);
            }

            // Include le relazioni necessarie
            query = query.Include(d => d.Utente)
                         .Include(d => d.Categoria)
                         .Include(d => d.SottoCategoria)
                         .Include(d => d.FileDocumenti);

            // Esegui la query e restituisci i risultati
            return await query.ToListAsync();

            //return await _context.Documenti
            //    .Where(d => d.UtenteId == documentiRequestFilter.UtenteId)  // Filtra per l'UtenteId
            //    .Include(d => d.Utente)               // Include i dettagli dell'Utente
            //    .Include(d => d.Categoria)            // Include i dettagli della Categoria
            //    .Include(d => d.FileDocumenti)        // Include i File associati
            //    .ToListAsync();

            // Per paginazione, inserendo come parametro (int pageNumber, int pageSize)
            //return await _context.Documenti
            //    .Include(d => d.Utente)
            //    .Include(d => d.Categoria)
            //    .Include(d => d.FileDocumenti)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToListAsync();
        }

        public async Task<Manage.Models.Documenti> GetByIdAsync(int id, string idUtente)
        {
            var documenti = await _context.Documenti
                                            .Include(d => d.Utente)
                                            .Include(d => d.Categoria)
                                            .Include(d => d.SottoCategoria)
                                            .Include(d => d.FileDocumenti)
                                            .FirstOrDefaultAsync(d => d.Id == id && d.UtenteId == idUtente);

            if (documenti == null)
            {
                throw new Exception($"Documento con id {id} non trovato.");
            }

            return documenti;
        }

        public async Task<Manage.Models.Documenti> AddAsync(Manage.Models.Request.DocumentiRequest documentiRequest, IFormFile[] newFiles)
        {
            // Recupero dell'utente associato (se esiste)
            var utente = await _context.Utenti.FindAsync(documentiRequest.UtenteId);
            if (utente == null)
            {
                throw new Exception("Utente non trovato");
            }

            // Recupero id della categoria in base alla sottocategoria
            var sottoCategoria = await _context.SottoCategoriaDocumenti.FindAsync(documentiRequest.SottoCategoriaDocumentiId);
            if (sottoCategoria == null)
            {
                throw new Exception("Sotto categoria non trovata");
            }

            // Recupero della categoria associata (se esiste)
            var categoria = await _context.CategoriaDocumenti.FindAsync(sottoCategoria.CategoriaDocumentiId);
            if (categoria == null)
            {
                throw new Exception("Categoria legata alla sottocategoria non trovata");
            }

            // Creazione del documento collegato all'utente e alla categoria
            var documenti = new Manage.Models.Documenti
            {
                Titolo = documentiRequest.TitoloDocumento,
                Descrizione = documentiRequest.DescrizioneDocumento,
                DataCreazioneDocumento = documentiRequest.DataCreazioneDocumento,
                DataInserimentoDocumento = DateTime.Now.ToLocalTime(),
                UtenteId = utente.Id, // Collegamento all'utente
                CategoriaDocumentiId = categoria.Id, // Collegamento alla categoria
                Categoria = categoria,
                SottoCategoriaDocumentiId = sottoCategoria.Id,
                SottoCategoria = sottoCategoria,
            };

            //// Gestione dei file
            //if (newFiles != null && newFiles.Length > 0)
            //{
            //    // Salva i nuovi file e aggiornali nel contesto
            //    var savedFiles = await _fileShared.SaveFilesAsync(newFiles, documentiRequest.UtenteId);
            //    foreach (var file in savedFiles)
            //    {
            //        documenti.FileDocumenti.Add(file);
            //    }
            //}

            // Aggiungi il documento al contesto
            _context.Documenti.Add(documenti);
            await _context.SaveChangesAsync(); // Salva per generare l'ID del document

            // Aggiunta dei file associati al documento
            if (newFiles != null && newFiles.Length > 0)
            {
                documenti.FileDocumenti = [];
                var savedFiles = await _fileShared.SaveFilesAsync(newFiles, documenti.UtenteId, documenti.Id);
                foreach (var file in savedFiles)
                {
                    documenti.FileDocumenti.Add(file);
                }
            }

            // Salva tutti i file e aggiorna il contesto
            await _context.SaveChangesAsync();

            return documenti;
        }

        public async Task<Manage.Models.Documenti> UpdateAsync(Manage.Models.Documenti documenti, List<int> idFilesDocumenti, IFormFile[] newFiles)
        {
            /*
                È buona pratica caricare il documento esistente dal database prima di aggiornarlo, per gestire correttamente le entità correlate (come i file) e 
                prevenire la perdita di dati.
             */
            // Carica il documento esistente dal database
            var existingDocumenti = await _context.Documenti
                .Include(d => d.FileDocumenti)  // Include i file esistenti
                .FirstOrDefaultAsync(d => d.Id == documenti.Id && d.UtenteId == documenti.UtenteId);

            if (existingDocumenti == null)
            {
                throw new Exception($"Documento con id {documenti.Id} non trovato.");
            }

            // Aggiorna i campi principali del documento
            existingDocumenti.Titolo = documenti.Titolo;
            existingDocumenti.Descrizione = documenti.Descrizione;
            existingDocumenti.DataCreazioneDocumento = documenti.DataCreazioneDocumento;
            existingDocumenti.UtenteId = documenti.UtenteId;
            existingDocumenti.CategoriaDocumentiId = documenti.CategoriaDocumentiId;
            existingDocumenti.Categoria = documenti.Categoria;

            existingDocumenti.SottoCategoriaDocumentiId = documenti.SottoCategoriaDocumentiId;
            existingDocumenti.SottoCategoria = documenti.SottoCategoria;

            // -------------------

            // Lista di ID file che devono essere mantenuti (dal client)
            var idFilesFromClient = idFilesDocumenti ?? new List<int>();

            // Cancella i file esistenti che non sono inclusi nella lista idFilesFromClient
            var filesToRemove = existingDocumenti.FileDocumenti
                .Where(f => !idFilesFromClient.Contains(f.Id))
                .ToList();

            if (filesToRemove.Any())
            {
                _fileShared.DeleteFiles(filesToRemove); // Cancella fisicamente i file
                _context.FileDocumenti.RemoveRange(filesToRemove); // Rimuovi dal database
            }

            // Gestione dei nuovi file da inserire
            if (newFiles != null && newFiles.Length > 0)
            {
                // Salva i nuovi file nel filesystem e aggiorna il database
                var savedFiles = await _fileShared.SaveFilesAsync(newFiles, existingDocumenti.UtenteId, documenti.Id);
                foreach (var file in savedFiles)
                {
                    existingDocumenti.FileDocumenti.Add(file); // Aggiungi i nuovi file alla relazione
                }
            }

            // -------------------

            // Gestione dei file
            //if (newFiles != null && newFiles.Length > 0)
            //{
            //    // Cancella i file esistenti
            //    _fileShared.DeleteFiles(existingDocumenti.FileDocumenti);

            //    // Rimuovi i file esistenti dal database
            //    _context.FileDocumenti.RemoveRange(existingDocumenti.FileDocumenti);

            //    // Salva i nuovi file e aggiornali nel contesto
            //    var savedFiles = await _fileShared.SaveFilesAsync(newFiles, existingDocumenti.UtenteId, documenti.Id);
            //    foreach (var file in savedFiles)
            //    {
            //        existingDocumenti.FileDocumenti.Add(file);
            //    }
            //}

            ////// Se l'utente ha fornito un nuovo elenco di file
            ////if (documenti.FileDocumenti != null && documenti.FileDocumenti.Any())
            ////{
            ////    // Identificare i file da rimuovere: quelli presenti in `existingDocumenti.FileDocumenti`
            ////    // ma non presenti in `documenti.FileDocumenti`.
            ////    // Caso in cui era presente 1 o più documenti e sono stati cambiati / cancellati
            ////    var filesToRemove = existingDocumenti.FileDocumenti
            ////        .Where(existingFile => !documenti.FileDocumenti
            ////            .Any(f => f.Id == existingFile.Id))
            ////        .ToList();

            ////    // Rimuovere i file che non sono più presenti
            ////    if (filesToRemove != null)
            ////    {
            ////        _context.FileDocumenti.RemoveRange(filesToRemove);

            ////        foreach (var file in filesToRemove)
            ////        {
            ////            if (!String.IsNullOrEmpty(file.PercorsoFile))
            ////            {
            ////                File.Delete(file.PercorsoFile);
            ////            }
            ////        }
            ////    }

            ////    // Aggiungere nuovi file: quelli presenti in `documenti.FileDocumenti`
            ////    // ma non ancora presenti in `existingDocumenti.FileDocumenti`.
            ////    foreach (var newFile in documenti.FileDocumenti)
            ////    {
            ////        // Aggiungi solo i file nuovi (non già esistenti)
            ////        if (!existingDocumenti.FileDocumenti.Any(f => f.Id == newFile.Id))
            ////        {
            ////            existingDocumenti.FileDocumenti.Add(new Manage.Models.FileDocumenti
            ////            {
            ////                NomeFile = newFile.NomeFile,
            ////                EstensioneFile = newFile.EstensioneFile,
            ////                PercorsoFile = newFile.PercorsoFile,
            ////                DocumentiId = existingDocumenti.Id
            ////            });
            ////        }
            ////    }
            ////}
            //else
            //{
            //    // Cancella i file esistenti
            //    _fileShared.DeleteFiles(existingDocumenti.FileDocumenti);

            //    // Rimuovi i file esistenti dal database
            //    _context.FileDocumenti.RemoveRange(existingDocumenti.FileDocumenti);
                
            //    //// Se non ci sono file nell'input (tutti i file devono essere rimossi)
            //    //_context.FileDocumenti.RemoveRange(existingDocumenti.FileDocumenti);

            //    //foreach (var file in existingDocumenti.FileDocumenti)
            //    //{
            //    //    if (!String.IsNullOrEmpty(file.PercorsoFile))
            //    //    {
            //    //        File.Delete(file.PercorsoFile);
            //    //    }
            //    //}
            //}

            // Aggiorna il documento nel contesto
            _context.Documenti.Update(existingDocumenti);
            await _context.SaveChangesAsync();

            return existingDocumenti;
        }

        public async Task<Manage.Models.Documenti> UpdateAsync(DocumentiDataPartial documenti, List<int> idFilesDocumenti, IFormFile[] newFiles, string utenteId)
        {
            /*
                È buona pratica caricare il documento esistente dal database prima di aggiornarlo, per gestire correttamente le entità correlate (come i file) e 
                prevenire la perdita di dati.
             */
            // Carica il documento esistente dal database
            var existingDocumenti = await _context.Documenti
                .Include(d => d.FileDocumenti)  // Include i file esistenti
                .Include(d => d.SottoCategoria) // Include la sotto-categoria
                .Include(d => d.Categoria)      // Include la categoria
                .FirstOrDefaultAsync(d => d.Id == documenti.Id && d.UtenteId == utenteId);
            if (existingDocumenti == null)
            {
                throw new Exception($"Documento con id {documenti.Id} non trovato.");
            }

            // Acquisire la SottoCategoria in base all'ID della sotto-categoria
            var sottoCategoria = await _context.SottoCategoriaDocumenti
                .FirstOrDefaultAsync(s => s.Id == documenti.SottoCategoriaDocumentiId);
            if (sottoCategoria == null)
            {
                throw new Exception($"Sotto-categoria con id {documenti.SottoCategoriaDocumentiId} non trovata.");
            }

            // Acquisire la Categoria associata alla sotto-categoria
            var categoria = await _context.CategoriaDocumenti
                .FirstOrDefaultAsync(c => c.Id == sottoCategoria.CategoriaDocumentiId);

            if (categoria == null)
            {
                throw new Exception($"Categoria associata alla sotto-categoria non trovata.");
            }

            // Ora puoi assegnare la categoria e la sotto-categoria al documento
            existingDocumenti.Titolo = documenti.Titolo;
            existingDocumenti.Descrizione = documenti.Descrizione;
            existingDocumenti.DataCreazioneDocumento = documenti.DataCreazioneDocumento;
            existingDocumenti.UtenteId = utenteId;
            existingDocumenti.CategoriaDocumentiId = categoria.Id;   // Assegna l'ID della categoria
            existingDocumenti.Categoria = categoria;                 // Assegna la categoria
            existingDocumenti.SottoCategoriaDocumentiId = sottoCategoria.Id; // Assegna l'ID della sotto-categoria
            existingDocumenti.SottoCategoria = sottoCategoria;             // Assegna la sotto-categoria

            // -------------------

            // Lista di ID file che devono essere mantenuti (dal client)
            var idFilesFromClient = idFilesDocumenti ?? new List<int>();

            // Cancella i file esistenti che non sono inclusi nella lista idFilesFromClient
            var filesToRemove = existingDocumenti.FileDocumenti
                .Where(f => !idFilesFromClient.Contains(f.Id))
                .ToList();

            if (filesToRemove.Any())
            {
                _fileShared.DeleteFiles(filesToRemove); // Cancella fisicamente i file
                _context.FileDocumenti.RemoveRange(filesToRemove); // Rimuovi dal database
            }

            // Gestione dei nuovi file da inserire
            if (newFiles != null && newFiles.Length > 0)
            {
                // Salva i nuovi file nel filesystem e aggiorna il database
                var savedFiles = await _fileShared.SaveFilesAsync(newFiles, existingDocumenti.UtenteId, documenti.Id);
                foreach (var file in savedFiles)
                {
                    existingDocumenti.FileDocumenti.Add(file); // Aggiungi i nuovi file alla relazione
                }
            }

            // Aggiorna il documento nel contesto
            _context.Documenti.Update(existingDocumenti);
            await _context.SaveChangesAsync();

            return existingDocumenti;
        }

        public async Task DeleteAsync(int id, string idUtente)
        {
            // Carica il documento esistente con i file associati e l'utente
            var documenti = await _context.Documenti
                                            .Include(d => d.FileDocumenti) // Include i file
                                            .Include(d => d.Utente)        // Include l'utente collegato
                                            .FirstOrDefaultAsync(d => d.Id == id && d.UtenteId == idUtente);

            if (documenti != null)
            {
                // Esegui eventuali controlli sull'utente, se necessario
                // Ad esempio, verifica se l'utente è autorizzato a cancellare il documento
                //if (/* Condizione sull'utente */ false)
                //{
                //    throw new UnauthorizedAccessException("Non hai i permessi per cancellare questo documento.");
                //}

                // Rimuovere i file associati prima di eliminare il documento
                if (documenti.FileDocumenti.Any())
                {
                    _context.FileDocumenti.RemoveRange(documenti.FileDocumenti);

                    // Cancella i file esistenti
                    _fileShared.DeleteFiles(documenti.FileDocumenti);
                }

                // Rimuovi il documento
                _context.Documenti.Remove(documenti);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Documento con id {id} non trovato.");
            }
        }
    }
}
