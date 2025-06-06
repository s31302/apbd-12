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
            var clinetWithPeselExist = await _dbService.ClinetWithPeselExist(clientTripDTO);
            if (!clinetWithPeselExist)
            {
                return BadRequest("Klient juz istnieje");
            }
            
            var tripWithIdExist = await _dbService.TripWithIdExist(idTrip, clientTripDTO);
            if (!tripWithIdExist)
            {
                return BadRequest("Wycieczka nie istnieje lub nazwa nie pasuje");
            }
            
            var dateTripOk = await _dbService.TripDateIsOk(idTrip);
            if (!dateTripOk)
            {
                return BadRequest("Data wycieczki jest przeszla");
            }
            
            await _dbService.SignClientOnTrip(idTrip, clientTripDTO);
            
            
            return StatusCode(200, "Klient zapisany na wycieczke");
        }
    }
}