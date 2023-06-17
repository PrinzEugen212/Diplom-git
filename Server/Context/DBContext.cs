using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using File = Server.Models.File;

namespace Server.Context
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<PublicLink> PublicLinks { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            builder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();
        }
    }
}
