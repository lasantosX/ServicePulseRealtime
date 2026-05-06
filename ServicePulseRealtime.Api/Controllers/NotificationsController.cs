using Microsoft.AspNetCore.Mvc;
using ServicePulseRealtime.Api.DTOs;
using ServicePulseRealtime.Api.Interfaces;

namespace ServicePulseRealtime.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetAll()
    {
        var notifications = await _notificationService.GetAllAsync();
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> Create(CreateNotificationDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required.");

        if (string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest("Message is required.");

        var notification = await _notificationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = notification.NotificationId }, notification);
    }

    [HttpPut("{id:int}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var updated = await _notificationService.MarkAsReadAsync(id);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}