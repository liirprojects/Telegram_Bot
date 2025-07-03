using Microsoft.EntityFrameworkCore;
using TelegramBot.Application.Entities;
namespace TelegramBot.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; } = null!;
}