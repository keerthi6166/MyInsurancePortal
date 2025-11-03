using Microsoft.EntityFrameworkCore;

namespace MyInsurancePortal.Models
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(c => c.ClaimId);

                entity.Property(c => c.ClaimNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(c => c.ClaimNumber)
                       .IsUnique(); //we cannot add the unique constrait right next to the isrequired as it belongs to the index function.

                entity.Property(c => c.Status)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Pending");

                entity.Property(c => c.Description)
                      .HasMaxLength(500);

                entity.Property(c => c.ClaimAmount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(c => c.Policy)
                      .WithMany(p => p.Claims)
                      .HasForeignKey(c => c.PolicyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);

                entity.Property(c => c.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(c => c.Email)
                        .IsUnique();                

                entity.Property(c => c.PhoneNumber)
                      .HasMaxLength(15);

                entity.Property(c => c.Address)
                      .HasMaxLength(200);

                entity.Property(c => c.DateOfBirth)
                      .IsRequired();

                // One-to-Many with Policy
                entity.HasMany(c => c.Policies)
                      .WithOne(p => p.Customer)
                      .HasForeignKey(p => p.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.Property(p => p.PaymentDate)
                      .IsRequired();

                entity.Property(p => p.AmountPaid)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(p => p.PaymentMode)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(p => p.TransactionId)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.HasIndex(p => p.TransactionId)
                      .IsUnique();
                

                entity.HasOne(p => p.Policy)
                      .WithMany(pol => pol.Payments)
                      .HasForeignKey(p => p.PolicyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.HasKey(p => p.PolicyId);

                entity.Property(p => p.PolicyNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(p => p.PolicyNumber)
                      .IsUnique();

                entity.Property(p => p.PolicyType)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.PremiumAmount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(p => p.StartDate)
                      .IsRequired();

                entity.Property(p => p.EndDate)
                      .IsRequired();

                entity.Property(p => p.Status)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Active");

                // Relationships
                entity.HasOne(p => p.Customer)
                      .WithMany(c => c.Policies)
                      .HasForeignKey(p => p.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Claims)
                      .WithOne(c => c.Policy)
                      .HasForeignKey(c => c.PolicyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Payments)
                      .WithOne(pay => pay.Policy)
                      .HasForeignKey(pay => pay.PolicyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure relationships if needed
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Policies)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Policy>()
                .HasMany(p => p.Claims)
                .WithOne(c => c.Policy)
                .HasForeignKey(c => c.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Policy>()
                .HasMany(p => p.Payments)
                .WithOne(pay => pay.Policy)
                .HasForeignKey(pay => pay.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
