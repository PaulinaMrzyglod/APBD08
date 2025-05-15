namespace Tutorial8.Models.DTOs;

public class ClientTripDTO
{
    //public ClientDTO Client { get; set; }
    public TripDTO Trip { get; set; }
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
    
    //public TripDTO Trip { get; set; }
    //public ClientDTO Client { get; set; }
}