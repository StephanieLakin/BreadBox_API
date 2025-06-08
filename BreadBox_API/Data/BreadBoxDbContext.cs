using Microsoft.EntityFrameworkCore;
using BreadBox_API.Entities;


namespace BreadBox_API.Data
{
    public class BreadBoxDbContext : DbContext
    {
        public BreadBoxDbContext(DbContextOptions<BreadBoxDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships with Users
            modelBuilder.Entity<User>()
                .HasMany(u => u.Clients)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.Leads)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Invoices)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Change to NO ACTION to avoid cascade path

            modelBuilder.Entity<User>()
                .HasMany(u => u.TimeEntries)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Change to NO ACTION to avoid cascade path

            // Relationships with Clients
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Client)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.TimeEntries)
                .WithOne(t => t.Client)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision for Amount fields
            modelBuilder.Entity<Invoice>()
                .Property(i => i.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValue(0m)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()"); // sets default to current UTC time
            });
        }

    }

}
