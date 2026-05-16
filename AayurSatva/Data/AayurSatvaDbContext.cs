using Microsoft.EntityFrameworkCore;
using AayurSatva.Models;

namespace AayurSatva.Data
{
    public class AayurSatvaDbContext : DbContext
    {
        public AayurSatvaDbContext(DbContextOptions<AayurSatvaDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<BloodGroup> BloodGroups { get; set; }
        public DbSet<MenuManager> MenuManagers { get; set; }
        public DbSet<UserMenuAccess> UserMenuAccesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Appointment relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Billing relationships
            modelBuilder.Entity<Billing>()
                .Property(b => b.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Billing>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicalRecord relationships
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Medicine precision
            modelBuilder.Entity<Medicine>()
                .Property(m => m.Price)
                .HasPrecision(10, 2);
                
            // History relationships
            modelBuilder.Entity<History>()
                .HasOne(h => h.User)
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}