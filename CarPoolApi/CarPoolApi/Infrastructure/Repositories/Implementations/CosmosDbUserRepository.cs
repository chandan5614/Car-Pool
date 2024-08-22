using Microsoft.Azure.Cosmos;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbUserRepository : IUserRepository
    {
        private readonly Container _container;

        public CosmosDbUserRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Entities.DTOs.User> GetByIdAsync(Guid userId)
        {
            try
            {
                // Query to find the document by UserId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @UserId")
                    .WithParameter("@UserId", userId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Entities.DTOs.User>(queryDefinition))
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
                Console.WriteLine($"Cosmos DB error: {ex.Message}, UserId: {userId}");
                throw;
            }

            return null;
        }

        public async Task<IEnumerable<Entities.DTOs.User>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Entities.DTOs.User>();
            var results = new List<Entities.DTOs.User>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task AddAsync(Entities.DTOs.User user)
        {
            await _container.CreateItemAsync(user, new PartitionKey(user.UserId.ToString()));
        }

        public async Task UpdateAsync(Entities.DTOs.User user)
        {
            await _container.UpsertItemAsync(user, new PartitionKey(user.UserId.ToString()));
        }

        public async Task DeleteAsync(Guid userId)
        {
            try
            {
                // Query to find the document by UserId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @UserId")
                    .WithParameter("@UserId", userId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Entities.DTOs.User>(queryDefinition))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        var user = response.FirstOrDefault();

                        if (user != null)
                        {
                            // Use the retrieved document's id for deletion
                            await _container.DeleteItemAsync<Entities.DTOs.User>(user.UserId.ToString("D"), new PartitionKey(user.UserId.ToString()));
                        }
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, UserId: {userId}");
                throw;
            }
        }

        public async Task<Entities.DTOs.User> GetByEmailAsync(string email)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Email = @Email")
                .WithParameter("@Email", email);
            var iterator = _container.GetItemQueryIterator<Entities.DTOs.User>(query);
            var results = new List<Entities.DTOs.User>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                if (response != null)
                {
                    results.AddRange(response);
                }
                else
                {
                    Console.WriteLine("Unexpected null response from Cosmos DB query.");
                }
            }

            return results.FirstOrDefault();
        }
    }
}
