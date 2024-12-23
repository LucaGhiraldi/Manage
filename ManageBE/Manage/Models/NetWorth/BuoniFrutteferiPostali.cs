using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class BuoniFruttiferiPostali : InvestimentoBase
    {
        public List<RendimentoBuoniFruttiferi> Rendimenti { get; set; } = new();
        public bool PenalitaPrelievoAnticipatoAttiva { get; set; } // Indica se applicare penalità
        public decimal PenalitaPercentuale { get; set; } // Penalità del 2% sui prelievi anticipati

        public BuoniFruttiferiPostali() { }

        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0; // Se non ci sono transazioni, il valore è 0

            decimal valoreTotale = 0;

            // Calcola il valore totale per ogni transazione di acquisto
            foreach (var transazione in transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto))
            {
                // Calcola il tempo effettivo in cui il buono è stato detenuto
                var tempoDetenzione = DateTime.Now - transazione.DataTransazione;

                // Ordina i rendimenti per durata crescente
                var rendimentiOrdinati = Rendimenti.OrderBy(r => r.Durata).ToList();

                decimal valoreBuono = transazione.Importo;

                // Applica i rendimenti in base alla durata di detenzione
                foreach (var rendimento in rendimentiOrdinati)
                {
                    if (tempoDetenzione <= TimeSpan.Zero)
                        break; // Se non c'è più tempo da considerare, interrompi il ciclo

                    // Calcola il periodo applicabile per questo rendimento
                    var periodoApplicabile = tempoDetenzione < rendimento.Durata ? tempoDetenzione : rendimento.Durata;

                    // Calcola il rendimento per il periodo applicabile
                    var anniFraziari = periodoApplicabile.TotalDays / 365.25;
                    valoreBuono *= (decimal)Math.Pow((double)(1 + rendimento.PercentualeRendimento), anniFraziari);

                    // Riduci il tempo di detenzione del periodo già applicato
                    tempoDetenzione -= periodoApplicabile;
                }

                // Somma il valore del buono corrente al totale
                valoreTotale += valoreBuono;
            }

            // Controlla se ci sono prelievi anticipati
            var prelievi = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita);

            if (prelievi.Any() && PenalitaPrelievoAnticipatoAttiva)
            {
                // Calcola la penalità sui prelievi anticipati
                var penalita = prelievi.Sum(p => p.Importo * PenalitaPercentuale);
                valoreTotale -= penalita;
            }

            return valoreTotale;
        }

        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Calcola l'importo totale versato per i buoni acquistati
            var importoTotaleVersato = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                                  .Sum(t => t.Importo);

            // Calcola il valore corrente del portafoglio
            var valoreCorrente = CalcolaValoreCorrente(transazioni);

            // Guadagno o perdita è la differenza tra valore corrente e importo versato
            return valoreCorrente - importoTotaleVersato;
        }

        public override string DescriviInvestimento()
        {
            return "Buoni fruttiferi postali con rendimenti crescenti.";
        }
    }
}
