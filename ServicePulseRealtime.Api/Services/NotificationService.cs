using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ServicePulseRealtime.Api.Data;
using ServicePulseRealtime.Api.DTOs;
using ServicePulseRealtime.Api.Hubs;
using ServicePulseRealtime.Api.Interfaces;
using ServicePulseRealtime.Api.Models;

namespace ServicePulseRealtime.Api.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationService(
        AppDbContext context,
        IHubContext<NotificationsHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<List<NotificationDto>> GetAllAsync()
    {
        return await _context.Notifications
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new NotificationDto
            {
                NotificationId = x.NotificationId,
                Title = x.Title,
                Message = x.Message,
                Type = x.Type,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            Type = dto.Type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var result = new NotificationDto
        {
            NotificationId = notification.NotificationId,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };

        await _hubContext.Clients.All.SendAsync("NotificationCreated", result);

        return result;
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);

        if (notification is null)
            return false;

        notification.IsRead = true;
        await _context.SaveChangesAsync();

        await _hubContext.Clients.All.SendAsync("NotificationUpdated", id);

        return true;
    }
}