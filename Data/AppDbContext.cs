using Microsoft.EntityFrameworkCore;
using MusicManagement.Models;

namespace MusicManagement.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Composer> Composers { get; set; }
}