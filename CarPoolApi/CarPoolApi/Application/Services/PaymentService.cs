using Application.DTOs;
using Application.Interfaces;
using Core.Interfaces;
using Entities.DTOs;

namespace Application.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(string paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(Guid.Parse(paymentId));
            return MapToDto(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(MapToDto);
        }

        public async Task AddPaymentAsync(PaymentDto paymentDto)
        {
            var payment = MapToEntity(paymentDto);
            await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentStatusAsync(string paymentId, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetByIdAsync(Guid.Parse(paymentId));
            payment.Status = status;
            await _paymentRepository.UpdateAsync(payment);
        }

        private static PaymentDto MapToDto(Payment payment)
        {
            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                RideId = payment.RideId,
                UserId = payment.UserId,
                Fare = payment.Fare,
                Status = payment.Status.ToString(),
                Timestamp = payment.Timestamp
            };
        }

        private static Payment MapToEntity(PaymentDto paymentDto)
        {
            return new Payment
            {
                PaymentId = paymentDto.PaymentId,
                RideId = paymentDto.RideId,
                UserId = paymentDto.UserId,
                Fare = paymentDto.Fare,
                Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), paymentDto.Status),
                Timestamp = paymentDto.Timestamp
            };
        }
    }
}