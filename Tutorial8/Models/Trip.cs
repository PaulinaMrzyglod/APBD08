namespace Tutorial8.Models;

public class Trip
{
    public int IdTrip { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    
    public List<Country> Countries { get; set; }
}

public class Country
{
    public int IdCountry { get; set; }
    public string Name { get; set; }

    // Relacja z wycieczkami
    public List<CountryTrip> CountryTrips { get; set; }
}

public class CountryTrip
{
    public int TripId { get; set; }
    public int CountryId { get; set; }
}