using BriefResume.Models;
using Microsoft.AspNetCore.Identity;
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

        //此处配置有问题,并非是一对一关系,而是单向关系,切记要改
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //配置Ablity 一对多
            //此处主要定义主键 外键关系,其余在模型Dto中定义
            builder.Entity<Ablity>()
                .HasOne<SeekerAttribute>(c => c.SeekerAttribute)
                .WithMany(c => c.Ablities)
                .IsRequired()
                .HasForeignKey(c => c.SeekerAttributeId);
            builder.Entity<Ablity>().HasKey(c => c.AblityId);

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

            builder.Entity<InterviewerAttribute>().HasKey(c=>c.InterviewerAttributeId);
            builder.Entity<SeekerAttribute>().HasKey(c => c.SeekerAttributeId);

            // 2. 添加角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"; // guid 
            builder.Entity<RoleExtension>().HasData(
                new RoleExtension
                {
                    Id = adminRoleId,
                    Name =RoleKey.superMoster.ToString(),
                    NormalizedName = RoleKey.superMoster.ToString().ToUpper()
                }
            );

            //添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            Seeker adminUser = new Seeker
            {
                Id = adminUserId,
                UserName = "adminNanaliz@qq.com",
                NormalizedUserName = "adminNanaliz@qq.com".ToUpper(),
                Email = "adminNanaliz@qq.com",
                NormalizedEmail = "adminNanaliz@qq.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            PasswordHasher<Seeker> ph = new PasswordHasher<Seeker>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "cc990327");
            builder.Entity<Seeker>().HasData(adminUser);

            // 3. 给用户加入管理员权限
            // 通过使用 linking table：IdentityUserRole
            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });
        }

    }
}
