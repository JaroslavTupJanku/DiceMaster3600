using DiceMaster3600.Data.Entitites;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace DiceMaster3600.Data
{
    public class SqlEFDataContext : DbContext
    {
        public DbSet<FacultyEntity> Faculties { get; set; }
        public DbSet<UniversityEntity> Universities { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Join(folder, "database.db");

            SqliteConnectionStringBuilder builder = new()
            {
                DataSource = path
            };

            optionsBuilder.UseSqlite(builder.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<UserEntity>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UniversityEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<UniversityEntity>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<FacultyEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<FacultyEntity>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<FacultyEntity>().HasOne(f => f.University).WithMany(u => u.Faculties).HasForeignKey(f => f.UniversityId);
            modelBuilder.Entity<UserEntity>().HasOne(u => u.Faculty).WithMany().HasForeignKey(u => u.FacultyId);
            modelBuilder.Entity<UserEntity>().HasOne(u => u.University).WithMany().HasForeignKey(u => u.UniversityId);
        }
    }
}
