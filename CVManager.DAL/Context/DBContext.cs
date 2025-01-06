using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CVManager.DAL.Entities;
using System.Reflection.Metadata;

namespace CVManager.DAL.Context
{
    public class CVContext(DbContextOptions<CVContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<CV> CVs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapping User to CV 1 to 1 relationship
            modelBuilder.Entity<User>()
                .HasOne(e => e.CV)
                .WithOne(e => e.User)
                .HasForeignKey<CV>();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention));
        }
    }
}
