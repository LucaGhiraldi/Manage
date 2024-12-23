namespace Manage.Models.NetWorth.DTO
{
    public class ObbligazioneDTO : InvestimentoDtoBase
    {
        public decimal CedolaAnnua { get; set; }  // Percentuale di cedola annuale
        public DateTime DataScadenza { get; set; } // Data di scadenza dell'obbligazione
        public bool HasPenalitaAnticipata { get; set; } // Indica se l'obbligazione è soggetta a penalità sui prelievi
        public decimal PenalitaAnticipataPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato (es. 0.02 per il 2%).
    }
}
