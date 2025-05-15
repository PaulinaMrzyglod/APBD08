using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models;

namespace Tutorial8.Repositories;

public interface IClientRepository
{
    Task<int> AddClientAsync(Client client, CancellationToken cancellationToken);
    Task<bool> ClientExistsAsync(int IdClient, CancellationToken cancellationToken);
    Task<bool> IsClientRegisteredAsync(int IdClient, int IdTrip, CancellationToken cancellationToken);
    Task RegisterClientToTripAsync(int IdClient,int IdTrip, CancellationToken cancellationToken);
}