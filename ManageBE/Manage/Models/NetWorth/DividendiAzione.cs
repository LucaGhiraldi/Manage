namespace Manage.Models.NetWorth
{
    public class DividendiAzione
    {
        public DateTime ExDividendDate { get; set; }
        public DateTime DeclarationDate { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
