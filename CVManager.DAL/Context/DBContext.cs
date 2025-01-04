using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CVManager.DAL.Entities;

namespace CVManager.DAL.Context
{
    public class CVContext : IdentityDbContext
    {
        public DbSet<CV> CVs { get; set; }

        public CVContext(DbContextOptions<CVContext> options)
           : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention));
        }
    }
}
