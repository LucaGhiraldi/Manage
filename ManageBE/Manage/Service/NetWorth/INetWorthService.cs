using Manage.Models.NetWorth.Base;
using Manage.Models.NetWorth.DTO;

namespace Manage.Service.NetWorth
{
    public interface INetWorthService
    {
        Task<IEnumerable<InvestimentoBase>> GetAllInvestimentiAsync(string idUtente);
        Task<InvestimentoBase> AddInvestimentoAsync(string idUtente, InvestimentoDtoBase investimentoDto);
    }
}
