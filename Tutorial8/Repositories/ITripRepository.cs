using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Repositories;

public interface ITripRepository
{
    Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<ClientTrip>> GetTripsByClientIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> TripExistsAsync(int IdTrip, CancellationToken cancellationToken);
    Task<bool> IsTripFullAsync(int IdTrip, CancellationToken cancellationToken);
}