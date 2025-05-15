using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips(CancellationToken cancellationToken)
        {
            var trips = await _tripsService.GetAllTripsAsync(cancellationToken);
            
            if (trips == null || !trips.Any())
            {
                return NotFound("No trips found.");
            }
            
            return Ok(trips);
        }

        [HttpGet("/api/clients/{clientId}/trips")]
        public async Task<IActionResult> GetTripAsync(int clientId, CancellationToken cancellationToken)
        {
            var trips = await _tripsService.GetTripsByClientIdAsync(clientId, cancellationToken);
            
            if (trips == null || !trips.Trips.Any())
            {
                return NotFound($"No trips found for client ID {clientId}.");
            }
            // if( await DoesTripExist(id)){
            //  return NotFound();
            // }
            // var trip = ... GetTrip(id);
            return Ok(trips);
        }
    }
}
