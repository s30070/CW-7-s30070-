using Microsoft.Data.SqlClient;
using TravelAgency.Models.DTOs;
using TravelAgency.Models.Entities;

namespace TravelAgency.Repositories
{
    public interface ITripRepository
    {
        Task<IEnumerable<TripResponse>> GetTripsAsync();
        Task<Trip?> GetTripAsync(int tripId, SqlTransaction? transaction = null);
        Task AssignClientAsync(int clientId, int tripId, DateTime date, SqlTransaction? transaction = null);
        Task<bool> RemoveClientAsync(int clientId, int tripId);
        Task<SqlTransaction> BeginTransactionAsync();
        Task<int> GetParticipantsCountAsync(int tripId);
        Task<bool> IsClientRegisteredAsync(int clientId, int tripId);
        Task<IEnumerable<ClientTripResponse>> GetClientTripsAsync(int clientId);
    }
}