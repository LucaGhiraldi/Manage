using Manage.Models.NetWorth.Enum;
using System.Text.Json.Serialization;

namespace Manage.Models.NetWorth.DTO
{
    public abstract class InvestimentoDtoBase
    {
        public string Nome { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public decimal PrezzoAttualeInvestimento { get; set; }
        public decimal PrezzoMedio { get; set; }
        public decimal PrezzoMinimo { get; set; }
        public decimal PrezzoMassimo { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]  // Questo converte la stringa in enum
        public TipoInvestimentoEnum TipoInvestimento { get; set; }

        public int UtenteId { get; set; }
    }
}
