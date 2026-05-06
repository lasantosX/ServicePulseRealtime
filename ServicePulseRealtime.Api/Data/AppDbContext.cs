using Microsoft.EntityFrameworkCore;
using ServicePulseRealtime.Api.Models;

namespace ServicePulseRealtime.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Notification> Notifications => Set<Notification>();
}