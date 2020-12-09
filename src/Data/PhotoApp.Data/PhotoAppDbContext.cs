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
        public PhotoAppDbContext(DbContextOptions<PhotoAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
