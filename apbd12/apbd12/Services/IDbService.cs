using apbd12.DTOs;

namespace apbd12.Services;

public interface IDbService
{ 
    Task<PagesDTO> GetTrips(int page = 1, int pageSize = 10);
    Task<bool> DeleteClient(int idClient);
    
    Task AddClientToTrip(int idClient, ClientTripDTO clientTripDTO);
    Task<bool> ClinetWithPeselExist(ClientTripDTO clientTripDTO);

    Task<bool> TripWithIdExist(int idTrip, ClientTripDTO clientTripDTO);
    Task<bool> TripDateIsOk(int idTrip);

    Task SignClientOnTrip(int idTrip, ClientTripDTO clientTripDTO);

}