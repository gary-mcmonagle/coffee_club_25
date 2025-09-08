using Microsoft.EntityFrameworkCore;

namespace CoffeeClub.ApiService.Infrastructure;

public class CoffeeClubDbContext : DbContext
{
    public CoffeeClubDbContext(DbContextOptions<CoffeeClubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Coffee> Coffees => Set<Coffee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coffee>().ToContainer("coffees");
        modelBuilder.Entity<Coffee>().HasPartitionKey(c => c.Id);
        base.OnModelCreating(modelBuilder);
    }
}


public class Coffee
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Origin { get; set; } = null!;
    public string Roast { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}