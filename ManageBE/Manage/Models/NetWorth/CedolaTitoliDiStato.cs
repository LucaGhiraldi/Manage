namespace Manage.Models.NetWorth
{
    public class CedolaTitoloDiStato
    {
        public int Id { get; set; }
        public int Anno { get; set; }
        public decimal PercentualeCedola { get; set; }

        // Relazione con TitoloDiStato
        public int TitoloDiStatoId { get; set; }
        public TitoloDiStato TitoloDiStato { get; set; }
    }
}
