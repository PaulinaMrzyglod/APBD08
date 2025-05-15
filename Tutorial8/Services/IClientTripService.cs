namespace Tutorial8.Services;

public interface IClientTripService
{
    Task<RegisterResult> RegisterClientToTripAsync(int IdTrip, int IdClient, CancellationToken cancellationToken);
    Task<bool> DeleteClientFromTripAsync(int IdTrip, int IdClient, CancellationToken cancellationToken);
}

public enum RegisterResult
{
    Success,
    NotFound,
    MaxReached,
    AlreadyRegistered
}