using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Models;

namespace ShowroomManagement.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) // Đổi thành ApplicationUser
    {
        public required DbSet<Vehicle> Vehicles { get; set; }
        public required DbSet<Customer> Customers { get; set; }
        public required DbSet<Order> Orders { get; set; }
         public required DbSet<Setting> Settings { get; set; }
        public required DbSet<VehicleRegistration> VehicleRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đảm bảo rằng các thuộc tính decimal có precision và scale phù hợp
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
