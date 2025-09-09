using CoffeeClub.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoffeeClub.ApiService.Infrastructure;

public class CoffeeClubDbContext : DbContext
{
    public CoffeeClubDbContext(DbContextOptions<CoffeeClubDbContext> options)
        : base(options)
    {
    }

    public DbSet<CoffeeClubCoffeeModel> Coffees => Set<CoffeeClubCoffeeModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoffeeClubCoffeeModel>().ToContainer("coffees");
        modelBuilder.Entity<CoffeeClubCoffeeModel>().HasPartitionKey(c => c.Id);
        base.OnModelCreating(modelBuilder);
    }
}


