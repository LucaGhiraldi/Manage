using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class ContoDeposito : InvestimentoBase
    {
        // Elenco dei tassi di interesse disponibili (durata e tasso associato).
        public List<TassoContoDeposito> Tassi { get; set; } = new();

        public bool HasPenalita { get; set; } // Indica se il conto ha penalità per prelievi anticipati
        public decimal PenalitaPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato (es. 0.02 per il 2%).  
        public decimal AliquotaTasse { get; set; } // Aliquota fiscale sugli interessi maturati (es. 0.26 per il 26%).        
        public decimal ImpostaBolloAnnuale { get; set; } // Percentuale di imposta di bollo annuale (es. 0.002 per lo 0.2%).    
        public decimal CostoGestioneFisso { get; set; } // Costi di apertura o gestione fissi (opzionali).

        public ContoDeposito() { }

        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0; // Se non ci sono transazioni, il valore corrente è 0.

            // Calcolo del capitale iniziale e dei prelievi effettuati
            var capitaleIniziale = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto).Sum(t => t.Quantita);
            var prelievi = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita).Sum(t => t.Quantita);
            var capitaleAttuale = capitaleIniziale - prelievi;

            // Se non c'è capitale rimanente, il valore corrente è 0
            if (capitaleAttuale <= 0)
                return 0;

            // Ordina i tassi di interesse per durata crescente
            Tassi = Tassi.OrderBy(t => t.Durata).ToList();

            // Calcola la durata totale dell'investimento in base alla data delle transazioni
            var dataInizio = transazioni.Min(t => t.DataTransazione);
            var dataFine = DateTime.Now;
            var durataTotale = dataFine - dataInizio;

            // Inizializza il valore corrente con il capitale attuale
            decimal valoreCorrente = capitaleAttuale;

            // Variabile per la durata rimanente da applicare (inizialmente è la durata totale)
            TimeSpan durataRimanente = durataTotale;

            // Calcola gli interessi composti per ciascun periodo di durata dei tassi di interesse
            foreach (var tasso in Tassi)
            {
                if (durataRimanente <= TimeSpan.Zero)
                    break; // Se non ci sono più periodi da calcolare, interrompi il ciclo.

                // Se la durata rimanente è inferiore alla durata del tasso corrente, calcola solo per il periodo rimanente
                var periodoApplicabile = durataRimanente < tasso.Durata ? durataRimanente : tasso.Durata;

                // Converte il periodo in anni frazionari (considerando gli anni bisestili)
                var anniFraziari = periodoApplicabile.TotalDays / 365.25;

                // Applica il tasso di interesse composto al valore corrente per il periodo applicabile
                valoreCorrente *= (decimal)Math.Pow((double)(1 + tasso.TassoInteresse), anniFraziari);

                // Riduci la durata rimanente del periodo già applicato
                durataRimanente -= periodoApplicabile;
            }

            // Se il conto prevede penalità, calcola le penalità per i prelievi anticipati
            if (HasPenalita)
            {
                // Calcola la penalità sui prelievi effettuati prima della scadenza del primo periodo
                var penalita = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita
                                                      && t.DataTransazione < dataInizio.Add(Tassi.First().Durata))
                                          .Sum(t => t.Quantita * PenalitaPercentuale);
                valoreCorrente -= penalita; // Sottrai la penalità dal valore corrente
            }

            // Calcola gli interessi maturati
            var interessiMaturati = valoreCorrente - capitaleIniziale;

            // Applica la tassa sugli interessi maturati
            valoreCorrente -= interessiMaturati * AliquotaTasse;

            // Calcola l'imposta di bollo annuale
            var impostaBollo = valoreCorrente * ImpostaBolloAnnuale * (decimal)durataTotale.TotalDays / 365.25m;
            valoreCorrente -= impostaBollo; // Sottrai l'imposta di bollo dal valore corrente

            // Sottrai il costo fisso di gestione annuale
            valoreCorrente -= CostoGestioneFisso;

            return valoreCorrente; // Restituisci il valore corrente finale
        }

        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Calcola il capitale iniziale, cioè l'importo totale investito in acquisti
            var capitaleIniziale = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                              .Sum(t => t.Importo);

            // Calcola il valore corrente attuale dell'investimento (inclusi interessi, penalità, ecc.)
            var valoreCorrente = CalcolaValoreCorrente(transazioni);

            // Variabile per la penalità sui prelievi anticipati (se il conto ha penalità)
            decimal penalita = 0;
            if (HasPenalita)
            {
                // Calcola la penalità se ci sono prelievi prima della scadenza
                penalita = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita
                                                  && t.DataTransazione < transazioni.Min(t => t.DataTransazione).Add(Tassi.First().Durata))
                                      .Sum(t => t.Quantita * PenalitaPercentuale);
            }

            // Calcola il guadagno o la perdita totale
            decimal guadagnoPerdita = valoreCorrente - capitaleIniziale - penalita;

            return guadagnoPerdita; // Restituisci il guadagno o la perdita
        }

        public override string DescriviInvestimento()
        {
            return "Conto deposito con tassi variabili.";
        }
    }
}
