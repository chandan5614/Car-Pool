using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbRatingRepository : IRatingRepository
    {
        private readonly Container _container;

        public CosmosDbRatingRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Rating> GetByIdAsync(Guid ratingId)
        {
            try
            {
                // Query to find the document by RatingId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.RatingId = @RatingId")
                    .WithParameter("@RatingId", ratingId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Rating>(queryDefinition))
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
                Console.WriteLine($"Cosmos DB error: {ex.Message}, RatingId: {ratingId}");
                throw;
            }

            return null;
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
            rating.RatingId = rating.RatingId;
            await _container.CreateItemAsync(rating, new PartitionKey(rating.RatingId.ToString()));
        }

        public async Task UpdateAsync(Rating rating)
        {
            rating.RatingId = rating.RatingId;
            await _container.UpsertItemAsync(rating, new PartitionKey(rating.RatingId.ToString()));
        }

        public async Task DeleteAsync(Guid ratingId)
        {
            try
            {
                // Query to find the document by RatingId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.RatingId = @RatingId")
                    .WithParameter("@RatingId", ratingId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Rating>(queryDefinition))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        var rating = response.FirstOrDefault();

                        if (rating != null)
                        {
                            // Use the retrieved document's id for deletion
                            await _container.DeleteItemAsync<Rating>(rating.RatingId.ToString("D"), new PartitionKey(rating.RatingId.ToString()));
                        }
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, RatingId: {ratingId}");
                throw;
            }
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
