using Microsoft.EntityFrameworkCore;

namespace Homework_57_izida_kubekova.Models.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Phone> Phones { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role() {Id = 1, Name = "admin"});
            modelBuilder.Entity<Role>().HasData(new Role() {Id = 2, Name = "user"});
            modelBuilder.Entity<User>().HasData(new User()
                {Id = 1, Email = "admin@admin.com", Name = "Admin", Password = "Admin678", RoleId = 1});
        }
    }
}