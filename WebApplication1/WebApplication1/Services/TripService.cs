using TravelAgency.Models.DTOs;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<IEnumerable<TripResponse>> GetTripsAsync()
        {
            return await _tripRepository.GetTripsAsync();
        }
    }
}