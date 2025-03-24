
using Microsoft.EntityFrameworkCore;
using SeventySevenDiamondsBackend;
using SeventySevenDiamondsBackend.Models;

public class AppDbContext : DbContext {
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Item> Items { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Fabric> Fabrics { get; set; }
    public DbSet<Image> Images { get; set; }

}