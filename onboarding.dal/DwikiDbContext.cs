using Microsoft.EntityFrameworkCore;
using onboarding.dal.Model;
using System;

namespace onboarding.dal
{
    public class DwikiDbContext : DbContext
    {
        public DwikiDbContext(DbContextOptions<DwikiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MovieModel>().HasIndex(t => t.Title).IsUnique();
        }

        public DbSet<MovieModel> Movie { get; set; }
    }
}
