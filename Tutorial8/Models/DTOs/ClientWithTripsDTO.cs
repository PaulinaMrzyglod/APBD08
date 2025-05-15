namespace Tutorial8.Models.DTOs;

public class ClientWithTripsDTO
{
    public ClientDTO Client { get; set; }
    public List<ClientTripDTO> Trips { get; set; }
}