using Microsoft.Data.SqlClient;
using Tutorial8.Models;
using Tutorial8.Models.DTOs;
using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{ 
    private readonly ITripRepository _tripsRepository;
    public TripsService(ITripRepository tripsRepository)
    {
        _tripsRepository = tripsRepository;
    }

    public async Task<IEnumerable<TripDTO>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var trips = await _tripsRepository.GetAllTripsAsync(cancellationToken);

        var tripsDTOs = trips.Select(trip => new TripDTO
        {
            Id = trip.IdTrip,
            Name = trip.Name,
            Countries = trip.Countries.Select(c => new CountryDTO { Name = c.Name }).ToList()
        }).ToList();
        
        return tripsDTOs;
    }

    public async Task<ClientWithTripsDTO> GetTripsByClientIdAsync(int clientId,
        CancellationToken cancellationToken)
    {
        var trips = await _tripsRepository.GetTripsByClientIdAsync(clientId, cancellationToken);
        var client = trips.First().Client;
        
        var result = new ClientWithTripsDTO()
        {
            Client = new ClientDTO
            {
                FirstName = client.FirstName,
                LastName = client.LastName
            },
            Trips = trips.Select(t => new ClientTripDTO
            {
                Trip = new TripDTO
                {
                    Id = t.Trip.IdTrip,
                    Name = t.Trip.Name,
                    Countries = t.Trip.Countries.Select(c => new CountryDTO { Name = c.Name }).ToList()
                },
                RegisteredAt = t.RegisteredAt,
                PaymentDate = t.PaymentDate
            }).ToList()
        };
        /*
        var tripsDTOs = trips.Select(ct => new ClientTripDTO
        {
            Client = new ClientDTO
            {
                FirstName = ct.Client.FirstName,
                LastName = ct.Client.LastName,
            },
            Trip = new TripDTO
            {
                Id = ct.Trip.IdTrip,
                Name = ct.Trip.Name,
                Countries = ct.Trip.Countries.Select(c => new CountryDTO { Name = c.Name }).ToList()
            },
            RegisteredAt = ct.RegisteredAt,
            PaymentDate = ct.PaymentDate
        }).ToList(); */
        
        return result;
    }
}