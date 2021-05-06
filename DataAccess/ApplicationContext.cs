using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<DistributionParameters> DistributionParameters { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Percentage> Percentages { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=transportationdb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Distribution>().HasKey(x => x.Id);
            modelBuilder.Entity<DistributionParameters>().HasKey(x => x.Id);
            modelBuilder.Entity<Experiment>().HasKey(x => x.Id);
            modelBuilder.Entity<Percentage>().HasKey(x => x.Id);

        }
    }
}
