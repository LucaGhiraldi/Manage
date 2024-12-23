namespace Manage.Models.NetWorth
{
    public class RendimentoBuoniFruttiferi
    {
        public int Id { get; set; }
        public TimeSpan Durata { get; set; }
        public decimal PercentualeRendimento { get; set; }

        // Relazione con BuoniFruttiferiPostali
        public int BuoniFruttiferiPostaliId { get; set; }
        public BuoniFruttiferiPostali BuoniFruttiferiPostali { get; set; }
    }
}
