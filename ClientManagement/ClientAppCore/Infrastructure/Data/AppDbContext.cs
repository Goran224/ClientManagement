using ClientAppCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientAppCore.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureClient(modelBuilder);
            ConfigureAddress(modelBuilder);
        }

        private void ConfigureClient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Addresses) 
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); 

        }

        private void ConfigureAddress(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(a => a.Type)
                .IsRequired();
        }
    }
}
