using Microsoft.EntityFrameworkCore;
using MedicineStoreAPI.Models; // Replace with your actual models namespace

namespace MedicineStoreAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        // Add other DbSet<T> for your models like Users, Orders, etc.
        public DbSet<User> Users { get; set; } // ✅ Add this line for JWT auth
    }
}
