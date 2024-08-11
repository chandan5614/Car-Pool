using Application.Interfaces;
using Infrastructure.Services.Implementations;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendNotificationAsync(string userId, string message)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationToAllUsersAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
