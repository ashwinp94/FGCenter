using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using FGCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FGCenter.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Game> Game { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Post> Post { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .Property(b => b.DatePosted)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ApplicationUser>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Post>()
                .Property(b => b.DatePosted)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User).WithMany(u => u.Comments).OnDelete(DeleteBehavior.Restrict);

            ApplicationUser user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                ImageUrl = "",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);


            modelBuilder.Entity<Game>().HasData(
               new Game {
                   GameId = 1,
                   Name = "Street Fighter V",
                   ImageUrl = "https://streetfighter.com/wp-content/uploads/2017/12/sfvae-logo.png",
                   DeveloperName = "Capcom" }
            );

            modelBuilder.Entity<Post>().HasData(
                new Post {
                    PostId = 1,
                    Title = "Bison Combos",
                    Text = "Vtrigger 2 combos",
                    UserId = user.Id,
                    GameId =1 }
            );
            modelBuilder.Entity<Comment>().HasData(
               new Comment
               {
                   CommentId = 1,
                   PostId = 1,
                   Text = "Vtrigger 2 for bison is really good",
                   UserId = user.Id,
               }
           );
        }
    }
}
