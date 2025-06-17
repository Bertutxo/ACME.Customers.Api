using ACME.Customers.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ACME.Customers.Infrastructure
{
    /// <summary>
    /// Contexto EF Core para ACME Customers.
    /// </summary>
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<SalesRep> SalesReps { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clientes
            modelBuilder.Entity<Client>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(200);
                e.Property(c => c.ContactEmail).IsRequired().HasMaxLength(200);
                e.Property(c => c.VisitDate).IsRequired();
                e.Property(c => c.Notes).HasMaxLength(1000);

                e.HasOne(c => c.SalesRep)
                 .WithMany(r => r.Clients)
                 .HasForeignKey(c => c.SalesRepId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Comerciales
            modelBuilder.Entity<SalesRep>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.Name).IsRequired().HasMaxLength(200);
                e.Property(r => r.Email).IsRequired().HasMaxLength(200);
                e.Property(r => r.Phone).HasMaxLength(50);
            });

            // Seed del comercial por defecto
            modelBuilder.Entity<SalesRep>().HasData(
                new SalesRep
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    Name = "Comercial Por Defecto",
                    Email = "default@acme.com",
                    Phone = null
                }
            );
        }
    }
}