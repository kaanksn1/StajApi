using Microsoft.EntityFrameworkCore;
using StajApi.Models;

namespace StajApi.Data;

/// <summary>
/// AppDbContext, Entity Framework Core'un veritabanı ile haberleşmesini sağlayan ana sınıftır.
/// Veritabanı bağlantı konfigürasyonlarını ve tabloların tanımını barındırır.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// DbContext constructor'ı. Veritabanı bağlantı seçenekleri (Connection String vb.)
    /// Dependency Injection (DI) aracılığıyla dışarıdan buraya aktarılır.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Users mülkü (DbSet), veritabanımızdaki "Users" tablosunu temsil eder.
    /// Kod içerisinde _context.Users diyerek veritabanı sorguları atabiliriz.
    /// </summary>
    public DbSet<User> Users { get; set; }
}