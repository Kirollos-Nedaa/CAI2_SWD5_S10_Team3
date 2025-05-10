using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Contexts
{
    class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=db18860.public.databaseasp.net; Database=db18860; User Id=db18860; Password=dF-98_bJwC@3; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");

        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            // Constructor logic here
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart_Items> Cart_Items { get; set; }
        public DbSet<Discount_Item> Discount_Items { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}
