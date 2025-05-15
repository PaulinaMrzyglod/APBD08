using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientTripController : ControllerBase
{
    private readonly IClientTripService _clientTripService;

    public ClientTripController(IClientTripService clientTripService)
    {
        _clientTripService = clientTripService;
    }

    [HttpPut("{id}/trips/{tripId}")]
    public async Task<IActionResult> AddClientToTrip(int IdClient, int IdTrip, 
        CancellationToken cancellationToken)
    {
        var result = await _clientTripService.RegisterClientToTripAsync(IdClient, IdTrip, cancellationToken);
        
        return result switch
        {
            RegisterResult.NotFound => NotFound("Client or trip not found"),
            RegisterResult.MaxReached => BadRequest("Maximum number of people reached for this trip"),
            RegisterResult.AlreadyRegistered => Conflict("Client already registered for this trip"),
            RegisterResult.Success => Ok("Client registered successfully"),
            _ => StatusCode(500, "Unexpected error")
        };

    }

    [HttpDelete("{id}/trips/{tripId}")]
    public async Task<IActionResult> DeleteClientFromTrip(int IdClient, int IdTrip, CancellationToken cancellationToken)
    {
        var result = await _clientTripService.DeleteClientFromTripAsync(IdClient, IdTrip, cancellationToken);

        if (!result)
        {
            return NotFound("Rejestracja klienta na tę wycieczkę nie istnieje.");
        }
        
        return NoContent();
    }
}