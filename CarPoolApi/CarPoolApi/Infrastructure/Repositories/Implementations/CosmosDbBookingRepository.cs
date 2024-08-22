using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.BookingId = @BookingId")
                    .WithParameter("@BookingId", bookingId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Booking>(query))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        return response.FirstOrDefault();
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, BookingId: {bookingId}");
                throw;
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
            booking.BookingId = booking.BookingId;
            await _container.CreateItemAsync(booking, new PartitionKey(booking.UserId.ToString()));
        }

        public async Task UpdateAsync(Booking booking)
        {
            booking.BookingId = booking.BookingId;
            await _container.UpsertItemAsync(booking, new PartitionKey(booking.UserId.ToString()));
        }

        public async Task DeleteAsync(Guid bookingId)
        {
            try
            {
                // Query to find the document by BookingId
                var query = new QueryDefinition("SELECT * FROM c WHERE c.BookingId = @BookingId")
                    .WithParameter("@BookingId", bookingId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Booking>(query))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        var booking = response.FirstOrDefault();

                        if (booking != null)
                        {
                            // Use the retrieved document's id for deletion
                            await _container.DeleteItemAsync<Booking>(booking.BookingId.ToString("D"), new PartitionKey(booking.UserId.ToString()));
                        }
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, BookingId: {bookingId}");
                throw;
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
    }
}
