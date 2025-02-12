using FakeFlightBookingAPI.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace FakeFlightBookingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Admin> AdminUsers { get; set; }
        public DbSet<Customer> CustomerUsers { get; set; }

        public DbSet<BookedFlight> BookedFlights { get; set; } // Represents the table


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sets precision and scale for decimal        
            modelBuilder.Entity<BookedFlight>().Property(b => b.Price).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<BookedFlight>()
                .HasOne<Customer>() // Ensures the relationship with Customer is maintained
                .WithMany()          // Assuming Customer can have many BookedFlights
                .HasForeignKey(bf => bf.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Or use Cascade/SetNull depending on your needs

            base.OnModelCreating(modelBuilder);
        }
    }
}
