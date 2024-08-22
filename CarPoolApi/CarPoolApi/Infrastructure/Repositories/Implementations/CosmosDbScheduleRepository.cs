using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbScheduleRepository : IScheduleRepository
    {
        private readonly Container _container;

        public CosmosDbScheduleRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Schedule> GetByIdAsync(Guid scheduleId)
        {
            try
            {
                // Query to find the document by ScheduleId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.ScheduleId = @ScheduleId")
                    .WithParameter("@ScheduleId", scheduleId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Schedule>(queryDefinition))
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
                Console.WriteLine($"Cosmos DB error: {ex.Message}, ScheduleId: {scheduleId}");
                throw;
            }

            return null;
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
            schedule.id = schedule.ScheduleId.ToString();
            await _container.UpsertItemAsync(schedule, new PartitionKey(schedule.ScheduleId.ToString()));
        }

        public async Task DeleteAsync(Guid scheduleId)
        {
            try
            {
                // Query to find the document by ScheduleId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.ScheduleId = @ScheduleId")
                    .WithParameter("@ScheduleId", scheduleId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Schedule>(queryDefinition))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        var schedule = response.FirstOrDefault();

                        if (schedule != null)
                        {
                            // Use the retrieved document's id for deletion
                            await _container.DeleteItemAsync<Schedule>(schedule.id, new PartitionKey(schedule.ScheduleId.ToString()));
                        }
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, ScheduleId: {scheduleId}");
                throw;
            }
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
