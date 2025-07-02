using Evernet.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Evernet.WebApi.Data;

public class EvernetDbContext(DbContextOptions<EvernetDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EvernetDbContext).Assembly);
    }
}