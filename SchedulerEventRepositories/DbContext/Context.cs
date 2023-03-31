using Microsoft.EntityFrameworkCore;
using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.DbContext;

public class Context : Microsoft.EntityFrameworkCore.DbContext
{
    public Context(DbContextOptions<Context> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Developer> Developers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }

}
