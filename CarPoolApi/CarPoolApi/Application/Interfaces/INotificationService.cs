namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message);
        Task SendNotificationToAllUsersAsync(string message);
    }
}
