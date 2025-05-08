// Na początku każdego pliku
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Models.DTOs;
using TravelAgency.Services;

namespace TravelAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            var result = await _clientService.GetClientTripsAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientRequest request)
        {
            var result = await _clientService.CreateClientAsync(request);
            return result.ToActionResult();
        }

        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> AssignToTrip(int id, int tripId)
        {
            var result = await _clientService.AssignToTripAsync(id, tripId);
            return result.ToActionResult();
        }

        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> RemoveFromTrip(int id, int tripId)
        {
            var result = await _clientService.RemoveFromTripAsync(id, tripId);
            return result.ToActionResult();
        }
    }
}