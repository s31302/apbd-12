using apbd12.DTOs;

namespace apbd12.Services;

public interface IDbService
{ 
    Task<PagesDTO> GetTrips(int page = 1, int pageSize = 10);
    Task DeleteClient(int idClient);
    
    Task AddClientToTrip(int idClient, ClientTripDTO clientTripDTO);
}