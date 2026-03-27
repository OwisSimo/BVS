using BVS.Models;
using Microsoft.EntityFrameworkCore;

namespace BVS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }
}