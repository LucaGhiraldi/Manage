using Manage.Data;
using Manage.Models.Data;
using Manage.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Manage.Controllers.Utente
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtenteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metodo per login
        [HttpPost("login")]
        public IActionResult GetUser([FromBody] LoginRequestValue request)
        {
            // Simula la verifica della password. Usa hashing per una sicurezza reale.
            var utente = _context.Utenti
                .FirstOrDefault(u => u.Email == request.Email && u.PasswordHash == request.Password);

            if (utente == null)
            {
                return Unauthorized("Email o password errata.");
            }

            LoginDataValue loginData = new();
            loginData.Id = utente.Id;
            //loginData.Nome = utente.Nome;
            //loginData.Cognome = utente.Cognome;
            loginData.Email = utente.Email;
            loginData.Password = utente.PasswordHash;
            loginData.DataInserimentoUtente = utente.DataInserimentoUtente;

            return Ok(loginData);
        }

    }
}
