using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface ITripsService
{
    Task<IEnumerable<TripDTO>> GetAllTripsAsync(CancellationToken cancellationToken);

    Task<ClientWithTripsDTO> GetTripsByClientIdAsync(int clientId,
        CancellationToken cancellationToken);
}