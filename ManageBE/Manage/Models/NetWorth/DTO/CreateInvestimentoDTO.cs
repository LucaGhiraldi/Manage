using Manage.Models.NetWorth.Enum;
using System.ComponentModel.DataAnnotations;

namespace Manage.Models.NetWorth.DTO
{
    public class CreateInvestimentoDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string Isin { get; set; } = string.Empty;

        [Required]
        public TipoInvestimentoEnum TipoInvestimento { get; set; } // Esempio: "TitoliDiStato", "ContoDeposito", etc.

        [Range(0, double.MaxValue)]
        public decimal PrezzoAttualeInvestimento { get; set; }
        public decimal PrezzoMedio { get; set; }
        public decimal PrezzoMinimo { get; set; }
        public decimal PrezzoMassimo { get; set; }

        // Proprietà opzionali o specifiche
        public List<(int Anno, decimal PercentualeCedola)> Cedole { get; set; } // Per Titoli di Stato
        public decimal BonusMantenimento { get; set; } // Percentuale bonus per il mantenimento senza prelievi
        public decimal PenalitaAnticipata { get; set; } // Percentuale di penalità per prelievi anticipati
        public decimal ValoreRimborso { get; set; } // Valore di rimborso del titolo a scadenza
        public decimal PenalitaPercentuale { get; set; } // Percentuale di penalità per prelievo anticipato (es. 0.02 per il 2%).
        public bool HasPenalita { get; set; } // Indica se il conto ha penalità per prelievi anticipati

        public List<(TimeSpan Durata, decimal TassoInteresse)> Tassi { get; set; } // Per Conto Deposito e Buoni Fruttiferi

        public decimal PercentualeStipendio { get; set; } // Per Fondo Pensione

        public decimal InteresseAnnuale { get; set; } // Per Fondo Pensione

        public string Settore { get; set; } // Per Azione
        public decimal DividendYield { get; set; } // Per Azione

        public decimal CedolaAnnua { get; set; } // Per Obbligazione
        public DateTime DataScadenza { get; set; } // Per Obbligazione

        public string Blockchain { get; set; } // Per Crytovaluta
        public decimal TassoStaking { get; set; } // Per Crytovaluta

        public string UtenteId { get; set; } // Foreign key per Utente
    }

}
