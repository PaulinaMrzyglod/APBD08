﻿namespace Tutorial8.Models;

public class ClientTrip
{
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
    
    public Trip Trip { get; set; }
    public Client Client { get; set; }
}