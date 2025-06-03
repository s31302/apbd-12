namespace apbd12.DTOs;

public class PagesDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripsDTO> Trips { get; set; }
}

public class TripsDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<CountriesDTO> Countries { get; set; }
    public List<ClientDTO> Clients { get; set; }
}

public class CountriesDTO
{
    public string Name { get; set; }
}

public class ClientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}