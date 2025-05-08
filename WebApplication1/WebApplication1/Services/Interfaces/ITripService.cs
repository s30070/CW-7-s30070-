using TravelAgency.Models.DTOs;

namespace TravelAgency.Services
{
    public interface ITripService
    {
        Task<IEnumerable<TripResponse>> GetTripsAsync();
    }
}