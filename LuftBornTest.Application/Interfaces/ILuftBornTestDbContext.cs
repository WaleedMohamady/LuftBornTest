using LuftBornTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuftBornTest.Application.Interfaces;
public interface ILuftBornTestDbContext
{
    DbSet<Product> Products { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
