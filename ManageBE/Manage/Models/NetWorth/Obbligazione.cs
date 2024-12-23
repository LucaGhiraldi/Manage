using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class Obbligazione : InvestimentoBase
    {
        public decimal CedolaAnnua { get; set; }  // Percentuale di cedola annuale
        public DateTime DataScadenza { get; set; } // Data di scadenza dell'obbligazione
        public bool HasPenalitaAnticipata { get; set; } // Indica se l'obbligazione è soggetta a penalità sui prelievi
        public decimal PenalitaAnticipataPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato (es. 0.02 per il 2%).

        public Obbligazione() { }

        // Calcola il valore corrente dell'obbligazione basandosi sulle transazioni, 
        // sulle cedole maturate e sul valore di mercato (se applicabile).
        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0; // Nessuna transazione registrata, il valore corrente è 0.

            // Capitale investito: somma del costo di tutte le transazioni di acquisto.
            var capitaleInvestito = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                               .Sum(t => t.Quantita * t.PrezzoUnitario);

            // Determina la durata dell'investimento in anni a partire dalla prima transazione di acquisto.
            var dataInizio = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                        .Min(t => t.DataTransazione);
            var durataInAnni = (DateTime.Now - dataInizio).TotalDays / 365.25;

            // Calcolo delle cedole maturate nel tempo
            decimal cedoleMaturate = 0;
            if (durataInAnni > 0)
            {
                // Calcola il numero di anni completi e la frazione di anno dalla data di inizio.
                int anniMaturi = (int)Math.Floor(durataInAnni); // Anni interi maturati.
                decimal frazioneAnno = (decimal)(durataInAnni - anniMaturi); // Periodo parziale.

                // Cedole maturate sugli anni completi.
                cedoleMaturate = (CedolaAnnua / 100) * capitaleInvestito * anniMaturi;

                // Aggiungi le cedole maturate per il periodo frazionario.
                cedoleMaturate += (CedolaAnnua / 100) * capitaleInvestito * frazioneAnno;
            }

            // Calcola il valore residuo del capitale investito (rimborso del capitale).
            // Se la data attuale è oltre la scadenza, il valore residuo non viene più considerato.
            var valoreResiduo = DateTime.Now < DataScadenza ? capitaleInvestito : 0;

            // Calcola il valore di mercato delle obbligazioni, basandosi sul prezzo attuale.
            var valoreDiMercato = transazioni.Sum(t => t.Quantita) * PrezzoAttualeInvestimento;

            // Somma il valore residuo e le cedole maturate per ottenere il valore corrente totale.
            var valoreCorrente = valoreResiduo + cedoleMaturate;

            // Confronta il valore corrente con il valore di mercato, e usa il più alto tra i due.
            if (PrezzoAttualeInvestimento > 0)
            {
                valoreCorrente = Math.Max(valoreCorrente, valoreDiMercato);
            }

            return valoreCorrente;
        }

        // Calcola il guadagno o la perdita totale basandosi sulle transazioni effettuate,
        // considerando il valore corrente, il capitale investito e eventuali penalità per prelievi anticipati.
        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Calcola il capitale investito: totale pagato per l'acquisto di obbligazioni.
            var capitaleInvestito = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                               .Sum(t => t.Quantita * t.PrezzoUnitario);

            // Calcola il valore corrente dell'obbligazione.
            var valoreCorrente = CalcolaValoreCorrente(transazioni);

            // Se l'obbligazione è soggetta a penalità sui prelievi anticipati:
            decimal penalita = 0;
            if (HasPenalitaAnticipata)
            {
                // Identifica i prelievi anticipati (prima della data di scadenza).
                var prelieviAnticipati = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita
                                                                && t.DataTransazione < DataScadenza)
                                                    .Sum(t => t.Quantita * t.PrezzoUnitario);

                // Calcola la penalità (percentuale configurabile).
                penalita = prelieviAnticipati * PenalitaAnticipataPercentuale;
            }

            // Guadagno o perdita complessiva: valore corrente meno capitale investito e penalità (se applicabile).
            decimal guadagnoPerdita = valoreCorrente - capitaleInvestito - penalita;

            return guadagnoPerdita;
        }

        // Descrizione specifica per le obbligazioni
        public override string DescriviInvestimento()
        {
            return $"Obbligazione: {Nome} ({Ticker}), Cedola Annua: {CedolaAnnua}%, Scadenza: {DataScadenza.ToShortDateString()}";
        }
    }
}
