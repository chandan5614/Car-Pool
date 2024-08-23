using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbBookingRepository : IBookingRepository
    {
        private readonly Container _container;

        public CosmosDbBookingRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Booking> GetByIdAsync(Guid bookingId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.BookingId = @BookingId")
                .WithParameter("@BookingId", bookingId.ToString());

            using var queryIterator = _container.GetItemQueryIterator<Booking>(query);
            if (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Booking>();
            var results = new List<Booking>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task AddAsync(Booking booking)
        {
            await _container.CreateItemAsync(booking, new PartitionKey(booking.UserId.ToString()));
        }

        public async Task UpdateAsync(Booking booking)
        {
            await _container.UpsertItemAsync(booking, new PartitionKey(booking.UserId.ToString()));
        }

        public async Task DeleteAsync(Guid bookingId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.BookingId = @BookingId")
                .WithParameter("@BookingId", bookingId.ToString());

            using var queryIterator = _container.GetItemQueryIterator<Booking>(query);
            if (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                var booking = response.FirstOrDefault();
                if (booking != null)
                {
                    await _container.DeleteItemAsync<Booking>(booking.BookingId.ToString("D"), new PartitionKey(booking.UserId.ToString()));
                }
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRideAsync(Guid rideId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.RideId = @RideId")
                .WithParameter("@RideId", rideId.ToString());

            var iterator = _container.GetItemQueryIterator<Booking>(query);
            var results = new List<Booking>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @UserId")
                .WithParameter("@UserId", userId.ToString());

            var iterator = _container.GetItemQueryIterator<Booking>(query);
            var results = new List<Booking>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Status = @Status")
                .WithParameter("@Status", status);

            var iterator = _container.GetItemQueryIterator<Booking>(query);
            var results = new List<Booking>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<int> GetBookingCountByRideAsync(Guid rideId)
        {
            var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.RideId = @RideId")
                .WithParameter("@RideId", rideId.ToString());

            using var queryIterator = _container.GetItemQueryIterator<int>(query);
            if (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return 0;
        }
    }
}
