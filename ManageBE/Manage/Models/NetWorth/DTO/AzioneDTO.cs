namespace Manage.Models.NetWorth.DTO
{
    public class AzioneDTO : InvestimentoDtoBase
    {
        public string Settore { get; set; }  // Settore dell'azienda (es. Tecnologia, Sanità, etc.)
        public bool IsAccumulo { get; set; }
        public decimal? DividendYield { get; set; } // Rendimento da dividendi (espresso in % | es. 0.05 per 5%)
    }
}
