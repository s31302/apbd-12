using apbd12.Data;
using apbd12.DTOs;
using apbd12.Models;
using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IDbService _dbService;
        private readonly Apbd12Context _dbContext;

        public ClientsController(IDbService dbService, Apbd12Context dbContext)
        {
            _dbService = dbService;
            _dbContext = dbContext;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var deleteClient = await _dbService.DeleteClient(idClient);
            if (!deleteClient)
            {
                return BadRequest("Klient ma wycieczki");
            }

            return Ok("Client deleted");
        }
        
    }
}