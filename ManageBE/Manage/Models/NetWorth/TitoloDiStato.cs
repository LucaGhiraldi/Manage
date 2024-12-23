using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class TitoloDiStato : InvestimentoBase
    {
        public List<CedolaTitoloDiStato> Cedole { get; set; } = new();

        public decimal BonusMantenimento { get; set; } // Percentuale bonus per il mantenimento senza prelievi
        public decimal PenalitaAnticipata { get; set; } // Percentuale di penalità per prelievi anticipati
        public decimal ValoreRimborso { get; set; } // Valore di rimborso del titolo a scadenza
        public decimal PenalitaPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato (es. 0.02 per il 2%).
        public bool HasPenalita { get; set; } // Indica se il conto ha penalità per prelievi anticipati

        public TitoloDiStato() { }

        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0; // Se non ci sono transazioni, il valore è 0.

            // Calcola la quantità totale di titoli acquistati e venduti
            var titoliAcquistati = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                              .Sum(t => t.Quantita);
            var titoliVenduti = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                           .Sum(t => t.Quantita);

            var titoliRimanenti = titoliAcquistati - titoliVenduti;

            if (titoliRimanenti <= 0)
                return 0; // Se non ci sono titoli rimanenti, il valore è 0.

            // Calcola la durata dell'investimento
            var dataInizio = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                        .Min(t => t.DataTransazione);
            var dataFine = DateTime.Now;
            var durataInAnni = (dataFine - dataInizio).TotalDays / 365.25;

            // Calcolo delle cedole accumulate
            decimal valoreCedole = 0;
            foreach (var cedola in Cedole)
            {
                // Somma le cedole per ogni anno, moltiplicando per i titoli rimanenti
                valoreCedole += titoliRimanenti * PrezzoAttualeInvestimento * cedola.PercentualeCedola / 100;
            }

            // Verifica se ci sono stati prelievi anticipati
            var prelieviAnticipati = transazioni.Any(t => t.TipoTransazione == TipoTransazione.Vendita && t.DataTransazione.Year < Cedole.First().Anno);
            decimal penalita = 0;

            if (prelieviAnticipati && HasPenalita)
            {
                // Calcola la penalità solo se `PenalitaAttiva` è true
                penalita = (valoreCedole + titoliRimanenti * PrezzoAttualeInvestimento) * PenalitaAnticipata;
            }

            // Calcola il valore totale
            decimal valoreTotale = (titoliRimanenti * PrezzoAttualeInvestimento) + valoreCedole - penalita;

            // Bonus extra se non ci sono stati prelievi anticipati
            decimal bonusExtra = 0;
            if (!prelieviAnticipati)
            {
                bonusExtra = valoreTotale * BonusMantenimento;
            }

            valoreTotale += bonusExtra;

            return valoreTotale;
        }

        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Calcola il capitale investito
            var capitaleInvestito = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                               .Sum(t => t.Importo);

            // Calcola il totale delle cedole maturate
            decimal totaleCedole = 0;
            foreach (var cedola in Cedole)
            {
                // Verifica se la cedola è maturata (anno corrente >= anno della cedola)
                var anniMaturi = transazioni.Any(t => t.DataTransazione.Year >= cedola.Anno) ? cedola.Anno : DateTime.Now.Year;
                if (anniMaturi >= cedola.Anno)
                {
                    totaleCedole += capitaleInvestito * cedola.PercentualeCedola / 100;
                }
            }

            // Verifica se ci sono stati prelievi anticipati
            var prelieviAnticipati = transazioni.Any(t => t.TipoTransazione == TipoTransazione.Vendita && t.DataTransazione.Year < Cedole.First().Anno);

            decimal valoreFinale = ValoreRimborso;
            if (prelieviAnticipati && HasPenalita)
            {
                // Applica la penalità solo se `PenalitaAttiva` è true
                valoreFinale -= valoreFinale * PenalitaAnticipata;
            }

            // Aggiungi il bonus di mantenimento se non ci sono stati prelievi
            if (!prelieviAnticipati)
            {
                valoreFinale += valoreFinale * BonusMantenimento;
            }

            // Calcola il guadagno o la perdita totale
            decimal guadagnoPerdita = totaleCedole + valoreFinale - capitaleInvestito;

            return guadagnoPerdita;
        }

        public override string DescriviInvestimento()
        {
            return "Titoli di Stato con cedole variabili.";
        }
    }
}
