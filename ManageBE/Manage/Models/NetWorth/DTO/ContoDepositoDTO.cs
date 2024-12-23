namespace Manage.Models.NetWorth.DTO
{
    public class ContoDepositoDTO : InvestimentoDtoBase
    {
        // Elenco dei tassi di interesse disponibili (durata e tasso associato).
        public List<TassoContoDeposito> Tassi { get; set; } = new();

        public bool HasPenalita { get; set; } // Indica se il conto ha penalità per prelievi anticipati
        public decimal PenalitaPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato (es. 0.02 per il 2%).  
        public decimal AliquotaTasse { get; set; } // Aliquota fiscale sugli interessi maturati (es. 0.26 per il 26%).        
        public decimal ImpostaBolloAnnuale { get; set; } // Percentuale di imposta di bollo annuale (es. 0.002 per lo 0.2%).    
        public decimal CostoGestioneFisso { get; set; } // Costi di apertura o gestione fissi (opzionali).
    }
}
