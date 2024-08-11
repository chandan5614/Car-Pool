using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbPaymentRepository : IPaymentRepository
    {
        private readonly Container _container;

        public CosmosDbPaymentRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            var response = await _container.ReadItemAsync<Payment>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Payment>();
            var results = new List<Payment>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task AddAsync(Payment payment)
        {
            await _container.CreateItemAsync(payment, new PartitionKey(payment.PaymentId.ToString()));
        }

        public async Task UpdateAsync(Payment payment)
        {
            await _container.UpsertItemAsync(payment, new PartitionKey(payment.PaymentId.ToString()));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _container.DeleteItemAsync<Payment>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<Payment> GetPaymentByRideAndUserAsync(Guid rideId, Guid userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.RideId = @RideId AND c.UserId = @UserId")
                .WithParameter("@RideId", rideId.ToString())
                .WithParameter("@UserId", userId.ToString());
            var iterator = _container.GetItemQueryIterator<Payment>(query);
            var results = await iterator.ReadNextAsync();
            return results.FirstOrDefault();
        }
    }
}
