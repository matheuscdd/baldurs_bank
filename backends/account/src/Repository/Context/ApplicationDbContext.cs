using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity => 
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Number).IsUnique();
            entity.HasIndex(u => u.UserId).IsUnique();
        });
        base.OnModelCreating(modelBuilder);
    }
}