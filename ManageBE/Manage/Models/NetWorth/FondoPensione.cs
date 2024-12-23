using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class FondoPensione : InvestimentoBase
    {
        public decimal PercentualeStipendio { get; set; } // Es.: 2%
        public decimal PercentualeStipendioDatoreLavoro { get; set; } // Es.: 4%
        public decimal InteresseAnnuale { get; set; }    // Es.: 4%
        public bool PenalitaPrelievoAnticipatoAttiva { get; set; } // Indica se applicare penalità sui prelievi anticipati
        public decimal PenalitaPrelievoPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato

        public FondoPensione() { }

        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0; // Se non ci sono transazioni, il valore è 0.

            decimal totale = 0;

            foreach (var transazione in transazioni)
            {
                // Calcola il tempo trascorso dall'investimento in anni
                var tempoInAnni = (DateTime.Now - transazione.DataTransazione).Days / 365.0;

                // Applica l'interesse composto al contributo
                totale += transazione.Importo * (decimal)Math.Pow(1 + (double)InteresseAnnuale / 100, tempoInAnni);
            }

            // Calcola l'ammontare totale dei contributi del lavoratore e del datore di lavoro
            var contributiTotali = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                              .Sum(t => t.Importo);

            // Controlla se ci sono prelievi anticipati
            var prelieviAnticipati = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita).Sum(t => t.Importo);

            // Applica la penalità sui prelievi anticipati, se attiva
            if (prelieviAnticipati > 0 && PenalitaPrelievoAnticipatoAttiva)
            {
                var penalita = prelieviAnticipati * PenalitaPrelievoPercentuale / 100;
                totale -= penalita; // Sottrai la penalità
            }

            return totale;
        }

        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Calcola l'importo totale versato dal lavoratore
            var importoTotaleVersato = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                                  .Sum(t => t.Importo);

            // Calcola il valore corrente del fondo
            var valoreCorrente = CalcolaValoreCorrente(transazioni);

            // Guadagno o perdita è la differenza tra valore corrente e contributi versati
            return valoreCorrente - importoTotaleVersato;
        }

        public override string DescriviInvestimento()
        {
            return "Fondo pensione basato su percentuale dello stipendio.";
        }
    }
}
