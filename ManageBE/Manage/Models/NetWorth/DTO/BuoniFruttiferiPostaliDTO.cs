namespace Manage.Models.NetWorth.DTO
{
    public class BuoniFruttiferiPostaliDTO : InvestimentoDtoBase
    {
        public List<RendimentoBuoniFruttiferi> Rendimenti { get; set; } = new();
        public bool PenalitaPrelievoAnticipatoAttiva { get; set; } // Indica se applicare penalità
        public decimal PenalitaPercentuale { get; set; } // Penalità del 2% sui prelievi anticipati
    }
}
