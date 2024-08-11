using Application.DTOs;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentByIdAsync(string paymentId);
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task AddPaymentAsync(PaymentDto payment);
        Task UpdatePaymentStatusAsync(string paymentId, PaymentStatus status);
    }
}
