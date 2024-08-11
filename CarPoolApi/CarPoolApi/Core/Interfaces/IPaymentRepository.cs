public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment> GetPaymentByRideAndUserAsync(Guid rideId, Guid userId);
}
