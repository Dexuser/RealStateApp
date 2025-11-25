using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.Contexts;

public class RealStateAppContext : DbContext 
{
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<SaleType> SaleTypes { get; set; }
    public DbSet<Improvement> Improvements { get; set; }
    public DbSet<PropertyImprovement> PropertyImprovements { get; set; }
    public DbSet<FavoriteProperty> FavoriteProperties { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Offer> Offers { get; set; }
    
    
    public RealStateAppContext(DbContextOptions<RealStateAppContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasSequence<long>("SeqPropertyCode")
            .StartsAt(1)
            .IncrementsBy(1)
            .HasMax(999999)
            .IsCyclic(false);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}