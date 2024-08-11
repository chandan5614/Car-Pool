using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbRatingRepository : IRatingRepository
    {
        private readonly Container _container;

        public CosmosDbRatingRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Rating> GetByIdAsync(Guid id)
        {
            var response = await _container.ReadItemAsync<Rating>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Rating>();
            var results = new List<Rating>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddAsync(Rating rating)
        {
            await _container.CreateItemAsync(rating, new PartitionKey(rating.RatingId.ToString()));
        }

        public async Task UpdateAsync(Rating rating)
        {
            await _container.UpsertItemAsync(rating, new PartitionKey(rating.RatingId.ToString()));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _container.DeleteItemAsync<Rating>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<IEnumerable<Rating>> GetRatingsByRideAsync(Guid rideId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.RideId = @RideId")
                .WithParameter("@RideId", rideId.ToString());
            var iterator = _container.GetItemQueryIterator<Rating>(query);
            var results = new List<Rating>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
    }
}
