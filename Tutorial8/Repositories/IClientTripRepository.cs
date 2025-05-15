namespace Tutorial8.Repositories;

public interface IClientTripRepository
{
    Task<bool> DeleteClientTripAsync(int IdClient, int IdTrip, CancellationToken cancellationToken);
}