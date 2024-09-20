using Microsoft.EntityFrameworkCore;

namespace Uzduotis01;

public class RentDBContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Bicycle> Bicycles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-OD4Q280;Database=CarRental;Integrated Security=True;TrustServerCertificate=true;");
    }
}
