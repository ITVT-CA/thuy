using Microsoft.EntityFrameworkCore;
using AuthServer.src.Models;

namespace AuthServer.src.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
}
