using Tutorial8.Models;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientService
{
    Task<int> AddClientAsync(ClientDTO clientDTO, CancellationToken cancellationToken);
}