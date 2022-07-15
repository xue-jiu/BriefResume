using BriefResume.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.DataBase
{
    public class UserDbContext : IdentityDbContext<Seeker, RoleExtension, string>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<SeekerAttribute> seekerAttributes { get; set; }
        public DbSet<Ablity> ablities { get; set; }
        public DbSet<InterviewerAttribute> interviewerAttributes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //配置Ablity 一对多
            //此处主要定义主键 外键关系,其余在模型Dto中定义
            builder.Entity<Ablity>()
                .HasOne<SeekerAttribute>(c => c.SeekerAttribute)
                .WithMany(c => c.Ablities)
                .IsRequired()
                .HasForeignKey(c => c.JobSeekerId);
            builder.Entity<Ablity>().HasKey(c => c.JobSeekerId);

            //seekerAttributes 与Seeker 一对一
            builder.Entity<Seeker>()
                .HasOne<SeekerAttribute>(c=>c.SeekerAttribute)
                .WithOne(c=>c.seeker)
                .HasForeignKey<SeekerAttribute>(c=>c.JobSeekerId);

            //InterviewerAttribute 与Seeker 一对一
            builder.Entity<Seeker>()
                .HasOne<InterviewerAttribute>(c => c.InterviewerAttribute)
                .WithOne(c => c.seeker)
                .HasForeignKey<InterviewerAttribute>(c=>c.InterviewerId);

            builder.Entity<InterviewerAttribute>().HasKey(c=>c.InterviewerId);
            builder.Entity<SeekerAttribute>().HasKey(c => c.JobSeekerId);

        }

    }
}
