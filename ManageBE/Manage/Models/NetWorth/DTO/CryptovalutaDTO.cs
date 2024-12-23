namespace Manage.Models.NetWorth.DTO
{
    public class CryptovalutaDTO : InvestimentoDtoBase
    {
        public string Blockchain { get; set; } // La blockchain di riferimento (es. Bitcoin, Ethereum)
        public decimal TassoStaking { get; set; } // Tasso annuale di rendimento in staking (es. 0.05 per 5%)
    }
}
