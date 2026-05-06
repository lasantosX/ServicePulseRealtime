using ServicePulseRealtime.Api.DTOs;

namespace ServicePulseRealtime.Api.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetAllAsync();
    Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
    Task<bool> MarkAsReadAsync(int id);
    Task<int> SeedSampleDataAsync();
}