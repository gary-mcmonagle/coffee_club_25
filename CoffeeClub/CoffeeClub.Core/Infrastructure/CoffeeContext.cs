using System;
using CoffeeClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeClub.Core.Infrastructure;

public class CoffeeContext : DbContext
{
    public CoffeeContext(DbContextOptions<CoffeeContext> options)
        : base(options)
    {
    }

    public DbSet<CoffeeEntity> Coffees => Set<CoffeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoffeeEntity>().ToContainer("coffees");
        modelBuilder.Entity<CoffeeEntity>().HasPartitionKey(c => c.Id);
        base.OnModelCreating(modelBuilder);
    }

}
