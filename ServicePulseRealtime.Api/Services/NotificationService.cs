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

    public async Task<int> SeedSampleDataAsync()
    {
        if (await _context.Notifications.AnyAsync())
            return 0;

        var notifications = new List<Notification>
    {
        new()
        {
            Title = "Repair Order Created",
            Message = "RO #10425 was created for customer John Smith.",
            Type = "Info",
            IsRead = false,
            CreatedAt = DateTime.UtcNow.AddMinutes(-20)
        },
        new()
        {
            Title = "Payment Processed",
            Message = "Customer payment was successfully processed.",
            Type = "Success",
            IsRead = false,
            CreatedAt = DateTime.UtcNow.AddMinutes(-15)
        },
        new()
        {
            Title = "Parts Delay",
            Message = "Parts request is waiting for vendor confirmation.",
            Type = "Warning",
            IsRead = false,
            CreatedAt = DateTime.UtcNow.AddMinutes(-10)
        },
        new()
        {
            Title = "Integration Error",
            Message = "Third-party system failed to respond.",
            Type = "Error",
            IsRead = true,
            CreatedAt = DateTime.UtcNow.AddMinutes(-5)
        }
    };

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        foreach (var notification in notifications.OrderByDescending(x => x.CreatedAt))
        {
            await _hubContext.Clients.All.SendAsync("NotificationCreated", new NotificationDto
            {
                NotificationId = notification.NotificationId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            });
        }

        return notifications.Count;
    }
}