using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbReportRepository : IReportRepository
    {
        private readonly Container _container;

        public CosmosDbReportRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Report> GetByIdAsync(Guid id)
        {
            var response = await _container.ReadItemAsync<Report>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Report>();
            var results = new List<Report>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddAsync(Report report)
        {
            await _container.CreateItemAsync(report, new PartitionKey(report.ReportId.ToString()));
        }

        public async Task UpdateAsync(Report report)
        {
            await _container.UpsertItemAsync(report, new PartitionKey(report.ReportId.ToString()));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _container.DeleteItemAsync<Report>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<IEnumerable<Report>> GetReportsByUserAsync(Guid userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @UserId")
                .WithParameter("@UserId", userId.ToString());
            var iterator = _container.GetItemQueryIterator<Report>(query);
            var results = new List<Report>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
    }
}
