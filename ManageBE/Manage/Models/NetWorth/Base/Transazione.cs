namespace Manage.Models.NetWorth.Base
{
    public class Transazione
    {
        public int Id { get; set; }

        // Relazione con l'investimento
        public int InvestimentoId { get; set; }
        public InvestimentoBase Investimento { get; set; }

        // Dettagli della transazione
        public DateTime DataTransazione { get; set; } // Quando è avvenuta la transazione
        public decimal PrezzoUnitario { get; set; }   // Prezzo per unità dell'investimento al momento
        public decimal Quantita { get; set; }         // Numero di unità (può essere negativo per vendite)
        public decimal Importo => PrezzoUnitario * Quantita; // Importo totale della transazione
        public decimal Commissione { get; set; }   // Prezzo per unità dell'investimento al momento

        // Tipo di transazione (esempio: Acquisto, Vendita)
        public TipoTransazione TipoTransazione { get; set; }
    }

    // Enum per rappresentare il tipo di transazione
    public enum TipoTransazione
    {
        Acquisto,
        Vendita,
        Altro
    }
}
