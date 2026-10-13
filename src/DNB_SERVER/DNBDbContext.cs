using Microsoft.EntityFrameworkCore;

public class DnbDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<VoteOption> VoteOptions => Set<VoteOption>();
    public DbSet<Schedule> Schedules => Set<Schedule>();

    public DnbDbContext(DbContextOptions<DnbDbContext> options) : base(options) { }
}