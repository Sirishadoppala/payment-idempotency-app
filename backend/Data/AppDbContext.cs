using Microsoft.EntityFrameworkCore;
using Payment_Idempotency_Service_Backend.Models;

namespace Payment_Idempotency_Service_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public AppDbContext() { }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Idempotency> Idempotency { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Idempotency>()
                .HasIndex(x => x.IdempotencyKey)
                .IsUnique();
            modelBuilder.Entity<Payment>()
                .HasIndex(x => x.PaymentId)
                .IsUnique();
        }
    }

}
