namespace Manage.Models.NetWorth.DTO
{
    public class ImmobileDTO : InvestimentoDtoBase
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
    }
}
