using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ShowroomManagement.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options) // Kế thừa IdentityDbContext
    {
        public required DbSet<Vehicle> Vehicles { get; set; }
        public required DbSet<Customer> Customers { get; set; }
        public required DbSet<Order> Orders { get; set; }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public required DbSet<IdentityRole> Roles { get; set; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đảm bảo rằng các thuộc tính decimal có precision và scale phù hợp
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18,2)");  // Chỉnh sửa theo yêu cầu của bạn

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");  // Chỉnh sửa theo yêu cầu của bạn
        }
    }
}
