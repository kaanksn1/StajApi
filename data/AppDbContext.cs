using Microsoft.EntityFrameworkCore;
using StajApi.Models.Entities;

namespace StajApi.Data;

/// <summary>
/// Entity Framework Core ile PostgreSQL arasındaki bağlantıyı ve tablo kurallarını tanımlar.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Veritabanındaki Users tablosunu temsil eder.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Id)
                .ValueGeneratedNever();

            entity.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(254);

            entity.HasIndex(user => user.Email)
                .IsUnique();

            entity.HasData(
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Fatih",
                    Email = "fatih.ulus@pointr.tech"
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Rüstem",
                    Email = "rustem.akkaya@pointr.tech"
                }
            );
        });
    }
}
