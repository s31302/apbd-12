using apbd12.Data;
using apbd12.DTOs;
using apbd12.Models;
using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private IDbService _dbService;
        private readonly Apbd12Context _dbContext;

        public TripsController(IDbService dbService, Apbd12Context dbContext)
        {
            _dbService = dbService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips(int page = 1, int pageSize = 10)
        {
            var allTrips = await _dbService.GetTrips(page, pageSize);
            
            return Ok(allTrips);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripDTO clientTripDTO)
        {
            await _dbService.AddClientToTrip(idTrip, clientTripDTO);
            
            return StatusCode(200, "Klient zapisany na wycieczke");
        }
    }
}