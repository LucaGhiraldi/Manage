using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class Cryptovaluta : InvestimentoBase
    {
        public string Blockchain { get; set; } // La blockchain di riferimento (es. Bitcoin, Ethereum)
        public decimal TassoStaking { get; set; } // Tasso annuale di rendimento in staking (es. 0.05 per 5%)

        public Cryptovaluta() { }

        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            if (!transazioni.Any())
                return 0;

            // Calcola la quantità totale
            var quantitaAcquistata = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                                .Sum(t => t.Quantita);
            var quantitaVenduta = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                             .Sum(t => t.Quantita);
            var quantitaRimanente = quantitaAcquistata - quantitaVenduta;

            if (quantitaRimanente <= 0)
                return 0;

            // Calcola la durata dell'investimento
            var dataInizio = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                        .Min(t => t.DataTransazione);
            var dataFine = DateTime.Now;
            var durataInAnni = (dataFine - dataInizio).TotalDays / 365.25;

            // Calcolo del guadagno da staking (compounding annuale)
            decimal valoreStaking = quantitaRimanente;
            if (TassoStaking > 0)
            {
                for (int i = 0; i < (int)Math.Floor(durataInAnni); i++)
                {
                    valoreStaking += valoreStaking * TassoStaking; // Reinvesti i guadagni annuali
                }

                // Guadagno per il periodo frazionario (se applicabile)
                var frazioneAnno = durataInAnni - Math.Floor(durataInAnni);
                if (frazioneAnno > 0)
                    valoreStaking += valoreStaking * TassoStaking * (decimal)frazioneAnno;
            }

            // Calcola il valore corrente netto
            var valoreCorrente = (valoreStaking * PrezzoAttualeInvestimento) - transazioni.Sum(t => t.Commissione);

            return valoreCorrente;
        }

        // Calcolo dei guadagni/perdite basato sulle transazioni e sul prezzo attuale
        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Capitale investito (somma degli importi delle transazioni di acquisto)
            var investimentoTotale = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                                .Sum(t => t.Importo);  // Importo totale degli acquisti

            // Capitale restituito (somma degli importi delle transazioni di vendita)
            var capitaleVenduto = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                              .Sum(t => t.Importo);  // Importo totale delle vendite

            // Valore attuale delle criptovalute possedute (quantità rimanente * prezzo attuale)
            var quantitaRimanente = transazioni.Where(t => t.TipoTransazione == TipoTransazione.Acquisto)
                                               .Sum(t => t.Quantita) -
                                    transazioni.Where(t => t.TipoTransazione == TipoTransazione.Vendita)
                                               .Sum(t => t.Quantita);

            if (quantitaRimanente <= 0)
                return 0; // Se non ci sono criptovalute rimanenti, ritorna 0

            var valoreAttuale = quantitaRimanente * PrezzoAttualeInvestimento;  // Calcolo del valore attuale

            // Calcolo dei guadagni da staking (compounding annuale)
            decimal valoreStaking = quantitaRimanente;
            var durataInAnni = (DateTime.Now - transazioni.Min(t => t.DataTransazione)).TotalDays / 365.25;

            if (TassoStaking > 0)
            {
                // Reinvestimento annuale dei guadagni (compounding)
                for (int i = 0; i < (int)Math.Floor(durataInAnni); i++)
                {
                    valoreStaking += valoreStaking * TassoStaking;  // Reinvesti i guadagni annuali
                }

                // Guadagno per la parte frazionale dell'anno
                var frazioneAnno = durataInAnni - Math.Floor(durataInAnni);
                if (frazioneAnno > 0)
                    valoreStaking += valoreStaking * TassoStaking * (decimal)frazioneAnno;
            }

            // Somma il valore dello staking al valore attuale
            var valoreConStaking = valoreStaking * PrezzoAttualeInvestimento;

            // Somma totale delle commissioni (sia acquisto che vendita)
            var commissioniTotali = transazioni.Sum(t => t.Commissione);

            // Calcola il guadagno o la perdita totale (valore attuale + guadagno da staking - commissioni)
            var guadagnoPerdita = valoreConStaking - investimentoTotale - capitaleVenduto - commissioniTotali;

            return guadagnoPerdita;
        }

        // Descrizione specifica per le criptovalute
        public override string DescriviInvestimento()
        {
            return $"Cryptovaluta: {Nome} ({Ticker}), Blockchain: {Blockchain}, Prezzo Attuale: {PrezzoAttualeInvestimento}";
        }
    }
}
