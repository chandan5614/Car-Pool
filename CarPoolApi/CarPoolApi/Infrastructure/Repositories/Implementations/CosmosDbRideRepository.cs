using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbRideRepository : IRideRepository
    {
        private readonly Container _container;

        public CosmosDbRideRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Ride> GetByIdAsync(Guid id)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.RideId = @rideId")
                    .WithParameter("@rideId", id.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Ride>(query))
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
                Console.WriteLine($"Cosmos DB error: {ex.Message}, ID: {id}");
                throw;
            }

            return null;
        }

        public async Task<IEnumerable<Ride>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Ride>();
            var results = new List<Ride>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddAsync(Ride ride)
        {
            await _container.CreateItemAsync(ride, new PartitionKey(ride.RideId.ToString()));
        }

        public async Task UpdateAsync(Ride ride)
        {
            await _container.UpsertItemAsync(ride, new PartitionKey(ride.RideId.ToString()));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _container.DeleteItemAsync<Ride>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<IEnumerable<Ride>> GetRidesByDriverAsync(Guid driverId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.DriverId = @DriverId")
                .WithParameter("@DriverId", driverId.ToString());
            var iterator = _container.GetItemQueryIterator<Ride>(query);
            var results = new List<Ride>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<IEnumerable<Ride>> SearchRidesAsync(string startLocation, string endLocation)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.StartLocation = @StartLocation AND c.EndLocation = @EndLocation")
                .WithParameter("@StartLocation", startLocation)
                .WithParameter("@EndLocation", endLocation);
            var iterator = _container.GetItemQueryIterator<Ride>(query);
            var results = new List<Ride>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
    }
}
