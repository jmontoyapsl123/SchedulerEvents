using Microsoft.EntityFrameworkCore;
using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.DbContext;

public class ContextApp : Microsoft.EntityFrameworkCore.DbContext
{
    public ContextApp(DbContextOptions<ContextApp> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Developer> Developers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventInvitation> EventInvitations { get; set; }
}
