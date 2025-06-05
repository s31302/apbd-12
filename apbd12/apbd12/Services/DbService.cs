using apbd12.Data;
using apbd12.DTOs;
using apbd12.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace apbd12.Services;

public class DbService : IDbService
{
    private readonly Apbd12Context _dbContext;

    public DbService(Apbd12Context dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PagesDTO> GetTrips(int page = 1, int pageSize = 10)
    {
        var allTrips = await _dbContext.Trips.CountAsync(); //liczy ile wycieczak
        var pages = (int)Math.Ceiling(allTrips/(double)pageSize); //liczy ile stron
        
        var getTrips = await _dbContext.Trips
            .Include(t => t.ClientTrips) //nawiguje do client trips
            .ThenInclude(ct => ct.IdClientNavigation)//nawiduje z client trips do client
            .Include(t => t.IdCountries) //nawiguje do countries
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize) // skipuje do numeru strony
            .Take(pageSize) //pobiera zawartosc wybranej strony
            .Select(t => new TripsDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountriesDTO
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();
        
        return new PagesDTO()
        {
            AllPages = pages, PageNum = page, PageSize = pageSize, Trips = getTrips
        };
    }

    public async Task DeleteClient(int idClient)
    {
        var client = await _dbContext.Clients.FindAsync(idClient);
        
        var clientTrip = await _dbContext.ClientTrips.CountAsync(ct => ct.IdClient == idClient);

        if (clientTrip > 0)
        {
            throw new BadRequestException("Klient ma wycieczki");
        }
        
        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddClientToTrip(int idTrip, ClientTripDTO clientTripDTO)
    {
        var clientWithPeselExist = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == clientTripDTO.Pesel);

        if (clientWithPeselExist != null)
        {
            throw new BadRequestException("Klient juz istnieje");
        }
        
        //po co sprawdzac czy kilent jest na wyczieczce jak juz sprawdzam ze jest w bazie danych 
        //lepiej by bylo chyba sprawdzac czy klient nie istnieje? 
        
        var clientTripWithPeselExist = await _dbContext.ClientTrips.FindAsync(clientWithPeselExist.IdClient, clientTripDTO.IdTrip);    
        
        if (clientTripWithPeselExist != null)
        {
            throw new BadRequestException("Klient juz istnieje zapisany na ta wycieczke");
        }
        
        var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);

        if (trip == null || trip.Name != clientTripDTO.TripName)
        {
            throw new BadRequestException("Wycieczka nie istnieje lub nazwa nie pasuje");
        }

        if (trip.DateFrom <= DateTime.UtcNow)
        {
            throw new BadRequestException("Data wycieczki jest przeszla");
        }
        
        var newClient = new Models.Client
        {
            FirstName = clientTripDTO.FirstName,
            LastName = clientTripDTO.LastName,
            Email = clientTripDTO.Email,
            Telephone = clientTripDTO.Telephone,
            Pesel = clientTripDTO.Pesel
        };

        _dbContext.Clients.Add(newClient);
        await _dbContext.SaveChangesAsync();

       
        var clientTrip = new Models.ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = clientTripDTO.IdTrip,
            RegisteredAt = DateTime.UtcNow,
            PaymentDate = clientTripDTO.PaymentDate
        };

        _dbContext.ClientTrips.Add(clientTrip);
        await _dbContext.SaveChangesAsync();
    }
}