using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class Azione : InvestimentoBase
    {
        public string Settore { get; set; }  // Settore dell'azienda (es. Tecnologia, Sanità, etc.)
        public bool IsAccumulo { get; set; }
        public decimal? DividendYield { get; set; } // Rendimento da dividendi (espresso in % | es. 0.05 per 5%)

        // Costruttore
        public Azione() { }

        // Implementazione specifica del calcolo del valore corrente del proprietario
        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0; // Se non ci sono transazioni, il valore è 0

            // Calcola il totale delle azioni acquistate e vendute
            var azioniAcquistate = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                               .Sum(t => t.Quantita);
            var azioniVendute = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                           .Sum(t => t.Quantita);

            // Azioni attualmente possedute
            var azioniRimanenti = azioniAcquistate - azioniVendute;

            if (azioniRimanenti <= 0)
                return 0; // Nessuna azione rimanente, quindi il valore è 0

            // Calcolo del NumeroAnniInvestimento (basato sulle date delle transazioni)
            var dataInizio = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                         .Min(t => t.DataTransazione);  // Prima data di acquisto
            var dataFine = transazioni.Any(t => t.TipoTransazione == TipoTransazione.Vendita)
                            ? transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                         .Max(t => t.DataTransazione)  // Ultima data di vendita
                            : DateTime.Now;  // Se non ci sono vendite, prendi la data attuale

            // Calcola la durata in anni
            var durataInAnni = (dataFine - dataInizio).TotalDays / 365.25;  // Usa 365.25 per considerare gli anni bisestili

            decimal guadagnoDividendo = 0;
            if (!IsAccumulo)
            {
                // Calcola il guadagno da dividendi (se in distribuzione)
                guadagnoDividendo = (decimal)(azioniRimanenti * PrezzoAttualeInvestimento * DividendYield);
            }
            else
            {
                // Calcolo del guadagno da dividendi per accumulo (reinvestiti)
                decimal capitaleAttuale = azioniRimanenti * PrezzoAttualeInvestimento;
                int anniInteri = (int)Math.Floor(durataInAnni); // Anni completi
                double frazioneAnno = durataInAnni - anniInteri; // Frazione di anno

                for (int i = 0; i < anniInteri; i++)
                {
                    // Ogni anno, i dividendi vengono reinvestiti
                    var dividendi = (decimal)(capitaleAttuale * DividendYield);
                    capitaleAttuale += dividendi;
                }

                // Calcola i dividendi per l'ultimo periodo parziale
                if (frazioneAnno > 0)
                {
                    var dividendiParziali = (decimal)(capitaleAttuale * DividendYield * (decimal)frazioneAnno);
                    capitaleAttuale += dividendiParziali;
                }

                guadagnoDividendo = capitaleAttuale - (azioniRimanenti * PrezzoAttualeInvestimento);
            }

            // Calcola le commissioni totali (sia acquisto che vendita)
            var commissioniTotali = transazioni.Sum(t => t.Commissione);

            // Calcola il valore corrente del portafoglio
            var valoreCorrente = (azioniRimanenti * PrezzoAttualeInvestimento) + guadagnoDividendo - commissioniTotali;

            return valoreCorrente;
        }

        // Calcolo dei guadagni/perdite basato sulle transazioni e sul prezzo attuale
        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Capitale investito (somma degli importi delle transazioni di acquisto)
            var investimentoTotale = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                                .Sum(t => t.Importo);  // Importo totale degli acquisti

            // Somma delle azioni rimanenti (azioni acquistate - azioni vendute)
            var azioniRimanenti = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                             .Sum(t => t.Quantita) -
                                  transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                             .Sum(t => t.Quantita);

            // Se non ci sono azioni rimanenti, il guadagno o la perdita è 0
            if (azioniRimanenti <= 0)
                return 0;

            // Valore attuale delle azioni possedute (azioni rimanenti * prezzo attuale)
            var valoreAttuale = azioniRimanenti * PrezzoAttualeInvestimento;

            // Calcolo dei dividendi (se applicabile)
            decimal dividendiTotali = 0;
            if (!IsAccumulo)
            {
                // Dividendi distribuiti (azione rimanenti * prezzo attuale * rendimento)
                dividendiTotali = (decimal)(azioniRimanenti * PrezzoAttualeInvestimento * DividendYield);
            }
            else
            {
                // Calcolo dividendi accumulati (reinvestiti ogni anno)
                decimal capitaleAccumulato = azioniRimanenti * PrezzoAttualeInvestimento;
                var anniDiInvestimento = (DateTime.Now - transazioni.Min(t => t.DataTransazione)).TotalDays / 365.25;

                for (int i = 0; i < (int)Math.Floor(anniDiInvestimento); i++)
                {
                    var dividendi = (decimal)(capitaleAccumulato * DividendYield);
                    capitaleAccumulato += dividendi;  // Reinvesti i dividendi
                }

                var dividendiParziali = (decimal)(capitaleAccumulato * DividendYield) * (decimal)(anniDiInvestimento - Math.Floor(anniDiInvestimento));
                capitaleAccumulato += dividendiParziali;

                dividendiTotali = capitaleAccumulato - (azioniRimanenti * PrezzoAttualeInvestimento);
            }

            // Somma totale dei dividendi e del valore attuale
            var valoreConDividendi = valoreAttuale + dividendiTotali;

            // Commissioni totali (acquisto e vendita)
            var commissioniTotali = transazioni.Sum(t => t.Commissione);

            // Calcola il guadagno/perdita totale (valore attuale + dividendi - investimento totale - commissioni)
            var guadagnoPerdita = valoreConDividendi - investimentoTotale - commissioniTotali;

            return guadagnoPerdita;
        }

        // Descrizione specifica per le azioni
        public override string DescriviInvestimento()
        {
            return $"Azione: {Nome} ({Ticker}), Settore: {Settore}, Prezzo Attuale: {PrezzoAttualeInvestimento}";
        }
    }
}
