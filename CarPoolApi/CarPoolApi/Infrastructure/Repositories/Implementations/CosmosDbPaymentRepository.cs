using Core.Interfaces;
using Entities.DTOs;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class CosmosDbPaymentRepository : IPaymentRepository
    {
        private readonly Container _container;

        public CosmosDbPaymentRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Payment> GetByIdAsync(Guid paymentId)
        {
            try
            {
                // Query to find the document by PaymentId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.PaymentId = @PaymentId")
                    .WithParameter("@PaymentId", paymentId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Payment>(queryDefinition))
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
                Console.WriteLine($"Cosmos DB error: {ex.Message}, PaymentId: {paymentId}");
                throw;
            }

            return null;
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
            payment.PaymentId = payment.PaymentId;
            await _container.CreateItemAsync(payment, new PartitionKey(payment.PaymentId.ToString()));
        }

        public async Task UpdateAsync(Payment payment)
        {
            payment.PaymentId = payment.PaymentId;
            await _container.UpsertItemAsync(payment, new PartitionKey(payment.PaymentId.ToString()));
        }

        public async Task DeleteAsync(Guid paymentId)
        {
            try
            {
                // Query to find the document by PaymentId
                var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.PaymentId = @PaymentId")
                    .WithParameter("@PaymentId", paymentId.ToString());

                using (var queryIterator = _container.GetItemQueryIterator<Payment>(queryDefinition))
                {
                    if (queryIterator.HasMoreResults)
                    {
                        var response = await queryIterator.ReadNextAsync();
                        var payment = response.FirstOrDefault();

                        if (payment != null)
                        {
                            // Use the retrieved document's id for deletion
                            await _container.DeleteItemAsync<Payment>(payment.PaymentId.ToString("D"), new PartitionKey(payment.PaymentId.ToString()));
                        }
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.Message}, PaymentId: {paymentId}");
                throw;
            }
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
