namespace Manage.Models.NetWorth
{
    public class TassoContoDeposito
    {
        public int Id { get; set; }
        public TimeSpan Durata { get; set; }
        public decimal TassoInteresse { get; set; }

        // Relazione con ContoDeposito
        public int ContoDepositoId { get; set; }
        public ContoDeposito ContoDeposito { get; set; }
    }
}
