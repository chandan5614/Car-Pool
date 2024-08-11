using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Core.Interfaces;
using Entities.DTOs;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbUserRepository : IUserRepository
    {
        private readonly Container _container;

        public CosmosDbUserRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Entities.DTOs.User> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _container.ReadItemAsync< Entities.DTOs.User >(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // Handle not found case
            }
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

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _container.DeleteItemAsync<Entities.DTOs.User>(id.ToString(), new PartitionKey(id.ToString()));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Handle not found case
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
                results.AddRange(response);
            }

            return results.FirstOrDefault();
        }
    }
}
