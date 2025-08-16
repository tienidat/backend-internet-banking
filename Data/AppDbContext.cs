using BPIBankSystem.API.DTOs;
using BPIBankSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPIBankSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }
        public DbSet<AddressChangeRequests> AddressChangeRequests { get; set; }
        public DbSet<CheckbookRequests> CheckbookRequests { get; set; }
        public DbSet<Checks> Checks { get; set; }
        public DbSet<StopPaymentRequests> StopPaymentRequests { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<CategoryHelp> CategoryHelps { get; set; }
        public DbSet<Help> Helps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("DECIMAL(18,2)");

            modelBuilder.Entity<TransferRequest>()
                .Property(t => t.Amount)
                .HasColumnType("DECIMAL(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("DECIMAL(18,2)");

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);


            modelBuilder.Entity<AddressChangeRequests>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("pending");

                entity.Property(e => e.RequestDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Notes)
                    .HasDefaultValue(string.Empty);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NewAddress)
                    .WithMany()
                    .HasForeignKey(e => e.NewAddressId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<CheckbookRequests>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("pending");

                entity.Property(e => e.RequestDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.CheckbookNumber)
                    .HasMaxLength(20)
                    .HasDefaultValue(string.Empty);

                entity.Property(e => e.Notes)
                    .HasDefaultValue(string.Empty);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Checks>(entity =>
            {
                entity.Property(e => e.CheckNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("issued");

                entity.Property(e => e.IssueDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.CancellationReason)
                    .HasMaxLength(255)
                    .HasDefaultValue(string.Empty);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<StopPaymentRequests>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("pending");

                entity.Property(e => e.RequestDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.CheckNumber)
                    .HasMaxLength(20)
                    .HasDefaultValue(string.Empty);

                entity.Property(e => e.Reason)
                    .HasMaxLength(255);

                entity.Property(e => e.Notes)
                    .HasDefaultValue(string.Empty);

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<CategoryHelp>().ToTable("CategoryHelps");
            modelBuilder.Entity<CategoryHelp>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<CategoryHelp>().Property(c => c.CategoryName).IsRequired();
            modelBuilder.Entity<Help>().ToTable("Helps");
            modelBuilder.Entity<Help>().HasKey(h => h.Id);
            modelBuilder.Entity<Help>().Property(h => h.Question).IsRequired();
            modelBuilder.Entity<Help>().Property(h => h.Answer).IsRequired();
            modelBuilder.Entity<Help>()
                .HasOne(h => h.Category)
                .WithMany(c => c.Helps)
                .HasForeignKey(h => h.CategoryId);

            modelBuilder.Entity<SupportRequest>(entity =>
            {
                entity.Property(sr => sr.Status)
                    .HasConversion<string>()
                    .HasDefaultValue("Pending");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
