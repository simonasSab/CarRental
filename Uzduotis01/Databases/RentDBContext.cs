using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uzduotis01
{
    public class RentDBContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Bicycle> Bicycles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("...");
        }
    }
}
