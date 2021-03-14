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

        public DbSet<UsersPhotoLikes> UsersPhotoLikes { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<UserWonChallange> UserWonChallanges { get; set; }

        public DbSet<Report> Reports { get; set; }

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

            builder.Entity<UsersPhotoLikes>()
                .HasKey(upl => new { upl.PhotoId, upl.UserId });

            builder.Entity<UsersPhotoLikes>()
                .HasOne<Photo>(upl => upl.Photo)
                .WithMany(p => p.UsersPhotoLikes)
                .HasForeignKey(upl => upl.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UsersPhotoLikes>()
                .HasOne<PhotoAppUser>(upl => upl.User)
                .WithMany(pau => pau.UsersPhotoLikes)
                .HasForeignKey(upl => upl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserNotification>()
                .HasKey(un => new { un.UserId, un.NotificationId });

            builder.Entity<UserNotification>()
                .HasOne<PhotoAppUser>(un => un.User)
                .WithMany(pau => pau.UserNotifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserNotification>()
                .HasOne<Notification>(un => un.Notification)
                .WithMany(n => n.UserNotifications)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserWonChallange>()
                .HasKey(uwc => new { uwc.UserId, uwc.ChallangeId });

            builder.Entity<UserWonChallange>()
                .HasOne<PhotoAppUser>(uwc => uwc.User)
                .WithMany(pau => pau.UserWonChallanges)
                .HasForeignKey(uwc => uwc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserWonChallange>()
                .HasOne<Challange>(uwc => uwc.Challange)
                .WithMany(c => c.UserWonChallanges)
                .HasForeignKey(uwc => uwc.ChallangeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
