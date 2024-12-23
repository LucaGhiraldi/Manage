namespace Manage.Models.NetWorth.DTO
{
    public class TitoloDiStatoDTO : InvestimentoDtoBase
    {
        public List<CedolaTitoloDiStato> Cedole { get; set; }
        public decimal BonusMantenimento { get; set; }
        public decimal PenalitaAnticipata { get; set; }
        public decimal ValoreRimborso { get; set; }
        public decimal PenalitaPercentuale { get; set; }
        public bool HasPenalita { get; set; } // Indica se il conto ha penalità per prelievi anticipati
    }
}
