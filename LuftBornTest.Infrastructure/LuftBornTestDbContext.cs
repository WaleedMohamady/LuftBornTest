using LuftBornTest.Application.Interfaces;
using LuftBornTest.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LuftBornTest.Infrastructure;
public class LuftBornTestDbContext(DbContextOptions<LuftBornTestDbContext> options) : IdentityDbContext<User>(options), ILuftBornTestDbContext
{
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LuftBornTestDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}



