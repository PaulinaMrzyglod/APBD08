using Tutorial8.Repositories;

namespace Tutorial8.Services;

public class ClientTripService : IClientTripService
{
    private readonly IClientRepository _clientRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IClientTripRepository _clientTripRepository;
    
    public ClientTripService(IClientRepository clientRepository, ITripRepository tripRepository, IClientTripRepository clientTripRepository)
    {
        _clientRepository = clientRepository;
        _tripRepository = tripRepository;
        _clientTripRepository = clientTripRepository;
    }


    public async Task<RegisterResult> RegisterClientToTripAsync(int IdTrip, int IdClient,
        CancellationToken cancellationToken)
    {
        if (!await _clientRepository.ClientExistsAsync(IdClient, cancellationToken) ||
            !await _tripRepository.TripExistsAsync(IdTrip, cancellationToken))
        {
            return RegisterResult.NotFound;
        }

        if (await _clientRepository.IsClientRegisteredAsync(IdClient, IdTrip, cancellationToken))
        {
            return RegisterResult.AlreadyRegistered;
        }

        if (await _tripRepository.IsTripFullAsync(IdTrip, cancellationToken))
        {
            return RegisterResult.MaxReached;
        }

        await _clientRepository.RegisterClientToTripAsync(IdClient, IdTrip, cancellationToken);
        return RegisterResult.Success;
    }

    public async Task<bool> DeleteClientFromTripAsync(int IdTrip, int IdClient, CancellationToken cancellationToken)
    {
        return await _clientTripRepository.DeleteClientTripAsync(IdTrip, IdClient, cancellationToken);
    }
}