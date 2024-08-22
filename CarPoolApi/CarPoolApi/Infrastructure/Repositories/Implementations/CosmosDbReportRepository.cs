using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbReportRepository : IReportRepository
    {
        private readonly Container _container;

        public CosmosDbReportRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Report> GetByIdAsync(Guid reportId)
        {
            try
            {
                // Query to find the document by ReportId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.ReportId = @ReportId")
                    .WithParameter("@ReportId", reportId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Report>(queryDefinition))
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
                Console.WriteLine($"Cosmos DB error: {ex.Message}, ReportId: {reportId}");
                throw;
            }

            return null;
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
            // Use PartitionKey based on ReportId
            await _container.CreateItemAsync(report, new PartitionKey(report.ReportId.ToString()));
        }

        public async Task UpdateAsync(Report report)
        {
            report.ReportId = report.ReportId;
            await _container.UpsertItemAsync(report, new PartitionKey(report.ReportId.ToString()));
        }

        public async Task DeleteAsync(Guid reportId)
        {
            try
            {
                // Query to find the document by ReportId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.ReportId = @ReportId")
                    .WithParameter("@ReportId", reportId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Report>(queryDefinition))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        var report = response.FirstOrDefault();

                        if (report != null)
                        {
                            // Use the retrieved document's id for deletion
                            await _container.DeleteItemAsync<Report>(report.ReportId.ToString("D"), new PartitionKey(report.ReportId.ToString()));
                        }
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, ReportId: {reportId}");
                throw;
            }
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
