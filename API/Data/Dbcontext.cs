using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<API.Entities.AppUser> Users => Set<API.Entities.AppUser>();
}
