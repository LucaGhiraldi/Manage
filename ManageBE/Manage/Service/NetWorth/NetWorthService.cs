using Manage.Data;
using Manage.Models.NetWorth.Base;
using Manage.Models.NetWorth.DTO;
using Manage.Models.NetWorth.Enum;
using Manage.Models.NetWorth.Factory;
using Manage.Shared;
using Microsoft.EntityFrameworkCore;

namespace Manage.Service.NetWorth
{
    public class NetWorthService : INetWorthService
    {
        private readonly ApplicationDbContext _context;
        private readonly MarketDataService _marketDataService;

        public NetWorthService(ApplicationDbContext context, MarketDataService marketDataService)
        {
            _context = context;
            _marketDataService = marketDataService;
        }

        public async Task<IEnumerable<InvestimentoBase>> GetAllInvestimentiAsync(string idUtente)
        {
            // Recupera gli investimenti dell'utente
            var investimenti = await _context.Investimenti
                .Where(i => i.UtenteId == idUtente)
                .Include(i => i.Transazioni) // Include le transazioni correlate
                .ToListAsync();

            // Aggiorna i dettagli per ogni investimento
            foreach (var investimento in investimenti)
            {
                await AggiornaPrezziECalcoli(investimento);
            }

            return investimenti;
        }

        public async Task<InvestimentoBase> AddInvestimentoAsync(string idUtente, InvestimentoDtoBase investimentoDto)
        {
            // Recupero dell'utente associato
            var utente = await _context.Utenti.FindAsync(idUtente);
            if (utente == null)
            {
                throw new Exception("Utente non trovato");
            }

            // Creazione dell'investimento usando la factory
            var investimento = InvestimentoFactory.CreateInvestimento(investimentoDto);

            // Associa l'investimento all'utente
            investimento.UtenteId = idUtente;

            // Aggiunta al contesto EF
            await _context.Investimenti.AddAsync(investimento);
            await _context.SaveChangesAsync();

            return investimento;
        }

        private async Task AggiornaPrezziECalcoli(InvestimentoBase investimento)
        {
            // Ottieni il prezzo attuale in base al tipo di investimento
            investimento.PrezzoAttualeInvestimento = investimento.TipoInvestimento switch
            {
                TipoInvestimentoEnum.Azioni => await _marketDataService.GetCurrentPriceStockAsyncFinancial(investimento.Ticker),
                // Da fare ETF
                TipoInvestimentoEnum.Cryptovalute => await _marketDataService.GetCurrentPriceCryptoAsync(investimento.Nome.ToLower()),
                _ => investimento.PrezzoAttualeInvestimento // Per altri tipi di investimenti
            };

            // Calcola il prezzo medio
            investimento.PrezzoMedio = CalcolaPrezzoMedio(investimento.Transazioni);

            // Aggiorna il valore corrente
            investimento.CalcolaValoreCorrente(investimento.Transazioni);
        }

        private decimal CalcolaPrezzoMedio(ICollection<Transazione> transazioni)
        {
            // Considera solo gli acquisti
            var acquisti = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto);

            // Calcola il totale investito (Prezzo per Unità * Unità + Commissioni)
            var totaleInvestito = acquisti.Sum(t => (t.PrezzoUnitario * t.Quantita) + t.Commissione);

            // Calcola il totale delle unità acquistate
            var totaleUnita = acquisti.Sum(t => t.Quantita);

            // Prezzo medio = Totale investito / Totale unità (se totale unità > 0)
            return totaleUnita > 0 ? totaleInvestito / totaleUnita : 0;
        }

    }
}
