using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=transportationdb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SupportSystemDatabase"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Distribution>().HasKey(x => x.Id);
            modelBuilder.Entity<DistributionParameters>().HasKey(x => x.Id);
            modelBuilder.Entity<Experiment>().HasKey(x => x.Id);
            modelBuilder.Entity<Percentage>().HasKey(x => x.Id);

            modelBuilder.Entity<Distribution>().HasData(new Distribution[] {
                new Distribution{Id = 1, Name = "Exponential"}, 
                new Distribution{Id = 2, Name = "Normal"}, 
                new Distribution{Id = 3, Name = "Uniform"}
            });

            modelBuilder.Entity<DistributionParameters>().HasOne(x => x.DistributionCs)
                .WithMany(y => y.DistributionParametersCs).HasForeignKey(k => k.DistributionCsId);
            modelBuilder.Entity<DistributionParameters>().HasOne(x => x.DistributionAB)
                .WithMany(y => y.DistributionParametersAB).HasForeignKey(k => k.DistributionABId);
            modelBuilder.Entity<DistributionParameters>().HasOne(x => x.DistributionL)
                .WithMany(y => y.DistributionParametersL).HasForeignKey(k => k.DistributionLId);
        }
    }
}
