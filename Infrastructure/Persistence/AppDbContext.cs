using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<List> Lists => Set<List>();
    public DbSet<Point> Points => Set<Point>();
    public DbSet<Square> Squares => Set<Square>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildListModel(modelBuilder);
        BuildPointModel(modelBuilder);
        BuildSquareModel(modelBuilder);
    }

    private static void BuildListModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<List>()
            .HasKey(list => list.Id);

        modelBuilder
            .Entity<List>()
            .HasMany(list => list.Points)
            .WithOne()
            .HasForeignKey(point => point.ListId);

        modelBuilder
            .Entity<List>()
            .HasMany(list => list.Squares)
            .WithOne()
            .HasForeignKey(square => square.ListId);
    }

    private static void BuildPointModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Point>()
            .HasKey(point => point.Id);

        modelBuilder
            .Entity<Point>()
            .HasIndex(point => new { point.ListId, point.X, point.Y })
            .IsUnique();
    }

    private static void BuildSquareModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Square>()
            .HasKey(square => square.Id);
    }
}
