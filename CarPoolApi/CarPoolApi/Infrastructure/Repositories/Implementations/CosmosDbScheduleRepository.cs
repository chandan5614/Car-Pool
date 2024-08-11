using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbScheduleRepository : IScheduleRepository
    {
        private readonly Container _container;

        public CosmosDbScheduleRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Schedule> GetByIdAsync(Guid id)
        {
            var response = await _container.ReadItemAsync<Schedule>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Schedule>();
            var results = new List<Schedule>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddAsync(Schedule schedule)
        {
            await _container.CreateItemAsync(schedule, new PartitionKey(schedule.ScheduleId.ToString()));
        }

        public async Task UpdateAsync(Schedule schedule)
        {
            await _container.UpsertItemAsync(schedule, new PartitionKey(schedule.ScheduleId.ToString()));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _container.DeleteItemAsync<Schedule>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(Guid userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @UserId")
                .WithParameter("@UserId", userId.ToString());
            var iterator = _container.GetItemQueryIterator<Schedule>(query);
            var results = new List<Schedule>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
    }
}
