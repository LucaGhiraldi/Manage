using Manage.Data;
using Manage.Models;
using Manage.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace Manage.Repository.CategoriaDocumenti
{
    public class CategoriaDocumentiRepository : ICategoriaDocumentiRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaDocumentiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Manage.Models.CategoriaDocumenti>> GetAllAsync(string idUtente)
        {
            //// Ottieni tutte le categorie predefinite e le categorie dell'utente
            //var categorie = await _context.CategoriaDocumenti
            //    .Where(c => c.IsPredefinita || c.UtenteId == idUtente)
            //    .ToListAsync();

            // Ottieni tutte le categorie predefinite e le categorie dell'utente, incluse le sotto-categorie
            var categorie = await _context.CategoriaDocumenti
                .Where(c => c.IsPredefinita || c.UtenteId == idUtente)
                .Include(c => c.SottoCategorie) // Include le sotto-categorie
                .ToListAsync();

            return categorie;
        }

        public async Task<Manage.Models.CategoriaDocumenti> GetByIdAsync(int id, string idUtente)
        {
            // Cerca la categoria per ID
            //// Possibile visualizzare solo la categoria non predefinita
            //var categoriaDocumenti = await _context.CategoriaDocumenti
            //    .Where(c => c.Id == id && c.UtenteId == idUtente && !c.IsPredefinita)
            //    .FirstOrDefaultAsync();

            // Cerca la categoria per ID, solo se non è predefinita e appartenente all'utente
            var categoriaDocumenti = await _context.CategoriaDocumenti
                .Where(c => c.Id == id && c.UtenteId == idUtente && !c.IsPredefinita)
                .Include(c => c.SottoCategorie) // Include le sotto-categorie
                .FirstOrDefaultAsync();

            return categoriaDocumenti;
        }

        public async Task<Manage.Models.CategoriaDocumenti> AddAsync(Manage.Models.Request.CategoriaDocumentiRequest categoriaDocumentiRequest, string idUtente)
        {
            // Recupero dell'utente associato (se esiste)
            var utente = await _context.Utenti.FindAsync(idUtente);
            if (utente == null)
            {
                throw new Exception("Utente non trovato");
            }

            // Creazione della categoria con i dati completi
            var categoriaDocumenti = new Manage.Models.CategoriaDocumenti
            {
                NomeCategoria = categoriaDocumentiRequest.NomeCategoria,
                DescrizioneCategoria = categoriaDocumentiRequest.DescrizioneCategoria,
                DataInserimentoCategoria = DateTime.Now,
                IsPredefinita = false,
                UtenteId = utente.Id
            };

            _context.CategoriaDocumenti.Add(categoriaDocumenti);
            await _context.SaveChangesAsync();

            // Creazione delle sotto-categorie, se esistono
            if (categoriaDocumentiRequest.SottoCategorie != null)
            {
                categoriaDocumenti.SottoCategorie = [];

                foreach (var sottoCategoriaRequest in categoriaDocumentiRequest.SottoCategorie)
                {
                    var sottoCategoria = new Manage.Models.SottoCategoriaDocumenti
                    {
                        NomeSottoCategoria = sottoCategoriaRequest.NomeSottoCategoria,
                        DescrizioneSottoCategoria = sottoCategoriaRequest.DescrizioneSottoCategoria,
                        DataInserimentoSottoCategoria = DateTime.Now,
                        CategoriaDocumentiId = categoriaDocumenti.Id  // associa la sotto-categoria alla categoria
                    };

                    // Aggiungi la sotto-categoria al contesto
                    categoriaDocumenti.SottoCategorie.Add(sottoCategoria);
                    //_context.SottoCategoriaDocumenti.Add(sottoCategoria);
                }
            }

            await _context.SaveChangesAsync();

            return categoriaDocumenti;
        }

        public async Task<Manage.Models.CategoriaDocumenti> UpdateAsync(Manage.Models.CategoriaDocumenti categoriaDocumenti)
        {
            // Verifica se la categoria appartiene all'utente e non è predefinita
            var existingCategoria = await _context.CategoriaDocumenti
                .Include(c => c.SottoCategorie) // Include le sotto-categorie per aggiornamenti
                .Where(c => c.Id == categoriaDocumenti.Id &&
                            c.UtenteId == categoriaDocumenti.UtenteId &&
                            !c.IsPredefinita)
                .FirstOrDefaultAsync();

            if (existingCategoria == null)
            {
                throw new Exception("Non puoi modificare una categoria predefinita o una categoria che non ti appartiene.");
            }

            // Aggiorna i dati della categoria
            existingCategoria.NomeCategoria = categoriaDocumenti.NomeCategoria;
            existingCategoria.DescrizioneCategoria = categoriaDocumenti.DescrizioneCategoria;

            // ---------------------------------------------------

            // 1. Rimuovi sotto-categorie che non sono più presenti nella lista aggiornata
            var sottoCategorieDaRimuovere = existingCategoria.SottoCategorie
                .Where(s => !categoriaDocumenti.SottoCategorie.Any(ns => ns.Id == s.Id && s.Id != 0)) // Confronta con la lista aggiornata, ignora ID nuovi (0)
                .ToList();

            foreach (var sottoCategoria in sottoCategorieDaRimuovere)
            {
                _context.SottoCategoriaDocumenti.Remove(sottoCategoria);
            }

            // 2. Aggiorna le sotto-categorie esistenti
            foreach (var sottoCategoriaEsistente in existingCategoria.SottoCategorie)
            {
                // Trova l'equivalente nella lista aggiornata
                var sottoCategoriaAggiornata = categoriaDocumenti.SottoCategorie
                    .FirstOrDefault(ns => ns.Id == sottoCategoriaEsistente.Id);

                if (sottoCategoriaAggiornata != null)
                {
                    // Sincronizza i campi della sotto-categoria
                    sottoCategoriaEsistente.NomeSottoCategoria = sottoCategoriaAggiornata.NomeSottoCategoria;
                    sottoCategoriaEsistente.DescrizioneSottoCategoria = sottoCategoriaAggiornata.DescrizioneSottoCategoria;
                }
            }

            // 3. Aggiungi le nuove sotto-categorie (quelle con ID pari a 0)
            var nuoveSottoCategorie = categoriaDocumenti.SottoCategorie
                .Where(ns => ns.Id == 0) // Considera solo le sotto-categorie con ID = 0
                .ToList();

            foreach (var nuovaSottoCategoria in nuoveSottoCategorie)
            {
                // Aggiungi nuove sotto-categorie alla lista
                existingCategoria.SottoCategorie.Add(new SottoCategoriaDocumenti
                {
                    NomeSottoCategoria = nuovaSottoCategoria.NomeSottoCategoria,
                    DescrizioneSottoCategoria = nuovaSottoCategoria.DescrizioneSottoCategoria,
                    DataInserimentoSottoCategoria = DateTime.Now // Imposta la data di creazione
                });
            }

            // Aggiorna la categoria nel contesto
            _context.CategoriaDocumenti.Update(existingCategoria);
            await _context.SaveChangesAsync();

            return existingCategoria;
        }

        public async Task DeleteAsync(int id, string idUtente)
        {
            // Verifica se esistono documenti collegati alla categoria
            var categoria = await _context.CategoriaDocumenti
                .Include(c => c.Documenti) // Include i documenti collegati
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                throw new Exception("Categoria non trovata.");
            }

            // Verifica se ci sono documenti collegati a questa categoria
            if (categoria.Documenti != null && categoria.Documenti.Any() && categoria.Documenti.Count > 0)
            {
                throw new Exception("Impossibile cancellare la categoria perché presente un collegamento con uno o più documenti.");
            }

            // Verifica se la categoria appartiene all'utente e non è predefinita
            var categoriaDocumenti = await _context.CategoriaDocumenti
                .Where(c => c.Id == id && c.UtenteId == idUtente && c.IsPredefinita == false)
                .FirstOrDefaultAsync();

            if (categoriaDocumenti == null)
            {
                throw new Exception("Non puoi cancellare una categoria predefinita o una categoria che non ti appartiene.");
            }

            _context.CategoriaDocumenti.Remove(categoriaDocumenti);
            await _context.SaveChangesAsync();
        }
    }
}
