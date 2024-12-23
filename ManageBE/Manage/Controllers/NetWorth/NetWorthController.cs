using Manage.Models;
using Manage.Models.NetWorth.DTO;
using Manage.Models.NetWorth.Enum;
using Manage.Models.NetWorth.Factory;
using Manage.Service.NetWorth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Manage.Controllers.NetWorth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NetWorthController : ControllerBase
    {
        private readonly INetWorthService _netWorthService;

        public NetWorthController(INetWorthService netWorthService)
        {
            _netWorthService = netWorthService;
        }

        [HttpGet("GetAllInvestimenti")]
        public async Task<IActionResult> GetAllInvestimenti()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var investimenti = await _netWorthService.GetAllInvestimentiAsync(userId);

            return Ok(investimenti);
        }

        [HttpPost("CreateInvestimento")]
        public async Task<IActionResult> CreateInvestimento([FromBody] InvestimentoDtoBase investimentoDto)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var investimento = await _netWorthService.AddInvestimentoAsync(userId, investimentoDto);

            return Ok(investimento);
        }

        [HttpGet("GetTipoInvestimento")]
        public IActionResult GetTipoInvestimento()
        {
            // Otteniamo tutti i valori dell'enum come oggetti TipoInvestimentoDto
            var tipoInvestimenti = Enum.GetValues(typeof(TipoInvestimentoEnum))
                .Cast<TipoInvestimentoEnum>()
                .Select(t => new TipoInvestimentoDTO
                {
                    Id = (int)t,  // Il valore numerico dell'enum
                    NomeInvestimento = t.ToString() // Il nome dell'enum come stringa
                })
                .ToList();

            return Ok(tipoInvestimenti);
        }
    }
}
