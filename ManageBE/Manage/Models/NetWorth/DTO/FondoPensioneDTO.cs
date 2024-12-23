namespace Manage.Models.NetWorth.DTO
{
    public class FondoPensioneDTO : InvestimentoDtoBase
    {
        public decimal PercentualeStipendio { get; set; } // Es.: 2%
        public decimal PercentualeStipendioDatoreLavoro { get; set; } // Es.: 4%
        public decimal InteresseAnnuale { get; set; }    // Es.: 4%
        public bool PenalitaPrelievoAnticipatoAttiva { get; set; } // Indica se applicare penalità sui prelievi anticipati
        public decimal PenalitaPrelievoPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato
    }
}
