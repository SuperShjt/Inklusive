using Microsoft.EntityFrameworkCore;
using Inklusive.Models;


namespace Inklusive.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProfileUpdate> ProfileUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Super Admin" },
                new Role { Id = 2, Name = "Admin" },
                new Role { Id = 3, Name = "Employee" }
            );
        }
    }
}

