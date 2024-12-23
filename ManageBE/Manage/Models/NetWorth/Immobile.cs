using Manage.Models.NetWorth.Base;

namespace Manage.Models.NetWorth
{
    public class Immobile : InvestimentoBase
    {
        // Prezzo iniziale di acquisto dell'immobile
        public decimal PrezzoAcquisto { get; set; }
        // Valore attuale dell'immobile (può variare nel tempo)
        public decimal ValoreAttuale { get; set; }
        // Reddito annuo da affitto
        public decimal RedditoAnnualeAffitto { get; set; }
        // Costi di gestione annuali per manutenzione, tasse, ecc.
        public decimal CostiGestioneAnnui { get; set; }
        // Aliquota fiscale sui guadagni da affitto o vendita
        public decimal AliquotaFiscale { get; set; }
        // Data di acquisto
        public DateTime DataAcquisto { get; set; }

        public Immobile() { }

        // Metodo per calcolare il guadagno/perdita
        public override decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni)
        {
            // Calcolare il capitale investito (solitamente è il prezzo di acquisto dell'immobile)
            var capitaleInvestito = PrezzoAcquisto;

            // Calcolare il reddito derivante dall'affitto durante il periodo di possesso
            var tempoDetenzione = DateTime.Now - DataAcquisto;
            decimal guadagnoAffitto = RedditoAnnualeAffitto * (decimal)(tempoDetenzione.TotalDays / 365.25);

            // Calcolare la plusvalenza o minusvalenza dalla vendita, se presente
            var valoreFinale = ValoreAttuale;
            decimal guadagnoVendita = 0;

            if (transazioni.Any(t => t.TipoTransazione == TipoTransazione.Vendita))
            {
                // Se l'immobile è stato venduto, calcola la plusvalenza o minusvalenza
                guadagnoVendita = valoreFinale - capitaleInvestito;
            }

            // Sottrarre le tasse sui guadagni (affitto e vendita)
            decimal tasseAffitto = guadagnoAffitto * AliquotaFiscale;
            decimal tasseVendita = guadagnoVendita * AliquotaFiscale;

            // Calcolare il guadagno netto totale (affitto + plusvalenza - costi di gestione - tasse)
            decimal guadagnoNetto = guadagnoAffitto - tasseAffitto + guadagnoVendita - tasseVendita - CostiGestioneAnnui;

            return guadagnoNetto;
        }

        // Metodo per calcolare il valore corrente dell'immobile (valore attuale più gli affitti)
        public override decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni)
        {
            // Calcolare il valore corrente dell'immobile come la somma del valore attuale e degli affitti guadagnati
            var tempoDetenzione = DateTime.Now - DataAcquisto;
            decimal valoreAffitto = RedditoAnnualeAffitto * (decimal)(tempoDetenzione.TotalDays / 365.25);

            // Consideriamo i costi di gestione annuali come un decremento del valores
            decimal valoreCorrente = ValoreAttuale + valoreAffitto - CostiGestioneAnnui;

            return valoreCorrente;
        }

        public override string DescriviInvestimento()
        {
            return "Immobile.";
        }
    }
}
