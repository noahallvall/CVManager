﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
            modelBuilder.Entity<CV>().HasData(
                new CV
                {
                    CVId = 1,
                    Summary = "Clark är en rolig grabb"
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
