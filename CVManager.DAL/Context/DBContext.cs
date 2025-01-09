using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CVManager.DAL.Entities;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;

namespace CVManager.DAL.Context
{
    public class CVContext(DbContextOptions<CVContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<CV> CVs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CVProject> CVProjects { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapping User to CV 1 to 1 relationship
            modelBuilder.Entity<User>()
                .HasOne(e => e.CV)
                .WithOne(e => e.User)
                .HasForeignKey<CV>();

            modelBuilder.Entity<CV>()
                .HasOne(c => c.User)
                .WithOne(u => u.CV)
                .HasForeignKey<CV>(c => c.UserId);
            modelBuilder.Entity<Skill>()
                 .HasOne<CV>()
                 .WithMany(cv => cv.Skills)
                .HasForeignKey(e => e.CVId);

            modelBuilder.Entity<Education>()
                 .HasOne<CV>()
                 .WithMany(cv => cv.Educations)
                .HasForeignKey(e => e.CVId);

            modelBuilder.Entity<Experience>()
                 .HasOne<CV>()
                 .WithMany(cv => cv.Experiences)
                .HasForeignKey(e => e.CVId);

            //Mapping the many to many relationsship between cv --> cvproject <-- project
            modelBuilder.Entity<CVProject>()
           .HasKey(cp => new { cp.CVId, cp.ProjectId });

           
            modelBuilder.Entity<CVProject>()
                .HasOne(cp => cp.CV)
                .WithMany(cv => cv.CVProjects)
                .HasForeignKey(cp => cp.CVId);

            modelBuilder.Entity<CVProject>()
                .HasOne(cp => cp.Project)
                .WithMany(p => p.CVProjects)
                .HasForeignKey(cp => cp.ProjectId);


            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    ProjectId = 1,
                    ProjectName = "MIB",
                    ProjectDescription = "En databas för Aliens"
                }
                );
   

            modelBuilder.Entity<Skill>().HasData(
                new Skill
                {
                    SkillId = 1,
                    SkillName = "C#",
                    CVId = 1
                },
                new Skill
                {
                    SkillId = 2,
                    SkillName = "JavaSript",
                    CVId = 1
                }

                );
            modelBuilder.Entity<Experience>().HasData(
                new Experience
                {
                    ExperienceId = 1,
                    CompanyName = "Walmart",
                    Role = "Casher",
                    CVId = 1
                },
                new Experience
                {
                    ExperienceId = 2,
                    CompanyName = "Google",
                    Role = "Programmer",
                    CVId = 1
                }
            );


            modelBuilder.Entity<Education>().HasData(
               new Education
               {
                   EducationId = 1,
                   Institution = "Orebro",
                   EducationName = "Systemvetenskap",
                   CVId = 1


               },
               new Education
               {
                   EducationId = 2,
                   Institution = "KTH",
                   EducationName = "Webbutvecklare",
                   CVId = 1

               }

               );





        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention));
        }
    }
}
