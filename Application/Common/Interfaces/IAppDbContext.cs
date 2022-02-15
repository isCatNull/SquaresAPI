using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;
public interface IAppDbContext
{
    DbSet<List> Lists { get; }
    DbSet<Point> Points { get; }
    DbSet<Square> Squares { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
