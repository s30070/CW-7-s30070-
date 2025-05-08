using TravelAgency.Models.DTOs;

namespace TravelAgency.Services
{
    public interface IClientService
    {
        Task<ServiceResult<IEnumerable<ClientTripResponse>>> GetClientTripsAsync(int clientId);
        Task<ServiceResult<int>> CreateClientAsync(ClientRequest request);
        Task<ServiceResult> AssignToTripAsync(int clientId, int tripId);
        Task<ServiceResult> RemoveFromTripAsync(int clientId, int tripId);
    }
}