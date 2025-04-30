using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.infrastructure
{
    public class AppDbContext : IdentityDbContext<User> /*DbContext*/ 
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology" },
                new Category { Id = 2, Name = "Health" },
                new Category { Id = 3, Name = "Sport" }
                );
            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Title = "C#", Content = "C# is a programming language", Image = "1.png", CreatedAt = new DateTime(2021, 3, 1), CategoryId = 1 },
                new  { Id = 2, Title = "Java", Content = "Java is a programming language", Image = "1.png", CreatedAt = new DateTime(2021, 3, 1), CategoryId = 1 },
                new Post { Id = 3, Title = "Python", Content = "Python is a programming language", Image = "1.png",CreatedAt = new DateTime(2021, 3, 1), CategoryId = 1 }
                );
            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, UserName = "Ali", Content = "Good", CommentDate = new DateTime(2021, 2, 1), PostId = 1 },
                new Comment { Id = 2, UserName = "Reza", Content = "Very Good", CommentDate = new DateTime(2021, 2, 1), PostId = 1 },
                new Comment { Id = 3, UserName = "Mina", Content = "Nice", CommentDate = new DateTime(2021, 2, 1), PostId = 1 }
                );




        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
            //optionsBuilder.UseSqlServer("Server=DESKTOP-0PQOEBL\\SQL2016;Database=S10_DB;Integrated Security=true;TrustServerCertificate=true")
            //    .UseLazyLoadingProxies();
        }
    }
}
