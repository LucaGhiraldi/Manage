using Manage.Models.NetWorth.Enum;
using System.Text.Json.Serialization;

namespace Manage.Models.NetWorth.Base
{
    public abstract class InvestimentoBase
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public decimal PrezzoAttualeInvestimento { get; set; }
        public decimal PrezzoMedio { get; set; }
        public decimal PrezzoMinimo { get; set; }
        public decimal PrezzoMassimo { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] // Deserializza correttamente la stringa come un enum
        public TipoInvestimentoEnum TipoInvestimento { get; set; } // Esempio: "TitoliDiStato", "ContoDeposito", etc.

        public string UtenteId { get; set; } // Foreign key per Utente
        public Utente Utente { get; set; } = null!;

        // Relazione con le transazioni
        public ICollection<Transazione> Transazioni { get; set; } = new List<Transazione>();

        // Logica comune: calcolo valore corrente del proprietario
        public abstract decimal CalcolaValoreCorrente(IEnumerable<Transazione> transazioni);

        // Logica personalizzata per guadagno/perdita
        public abstract decimal CalcolaGuadagnoPerdita(IEnumerable<Transazione> transazioni);

        // Logica specifica del tipo di investimento
        public abstract string DescriviInvestimento();
    }
}
