using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data
{
    public class PhotoAppDbContext : IdentityDbContext<PhotoAppUser>
    {
        public DbSet<Challange> Challanges { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<PhotoChallange> PhotosChallanges { get; set; }

        public DbSet<UserPhoto> UsersPhotos { get; set; }

        public DbSet<AccountUserPhoto> AccountsUsersPhotos { get; set; }

        public PhotoAppDbContext(DbContextOptions<PhotoAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<PhotoAppUser>()
                .HasOne<AccountUserPhoto>(u => u.ProfilePicture)
                .WithOne(aup => aup.User)
                .HasForeignKey<AccountUserPhoto>(aup => aup.UserId);

            builder.Entity<UserPhoto>()
                .HasKey(up => new { up.UserId, up.PhotoId });

            builder.Entity<UserPhoto>()
                .HasOne(up => up.User)
                .WithMany(u => u.UsersPhotos)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserPhoto>()
                .HasOne(up => up.Photo)
                .WithMany(p => p.UsersPhotos)
                .HasForeignKey(up => up.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PhotoChallange>()
                .HasKey(ph => new { ph.PhotoId, ph.ChallangeId });

            builder.Entity<PhotoChallange>()
                .HasOne(pc => pc.Photo)
                .WithMany(p => p.PhotosChallanges)
                .HasForeignKey(pc => pc.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PhotoChallange>()
                .HasOne(pc => pc.Challange)
                .WithMany(c => c.PhotosChallanges)
                .HasForeignKey(pc => pc.ChallangeId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
