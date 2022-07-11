using BriefResume.DataBase;
using BriefResume.JwtModel;
using BriefResume.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BriefResume
{
    public class Startup
    {
        public IConfiguration _confirguration { get; }
        public Startup(IConfiguration configuration)
        {
            _confirguration = configuration;
        }
        //This method gets called by the runtime.Use this method to add services to the container.
        //For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Dbcontextע��
            services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlServer(_confirguration["DbContext:ConnectionString"]);//�õ�����string
            });
            services.AddDataProtection();

            services.AddIdentityCore<JobSeeker>(opt => //��AddIdentity��ͬ,AddIdentity�������Ĭ�Ͻ���
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                //�������ɹ���
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;//��������ʱ,���ɽ�Ϊ�򵥵�����
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                .AddRoles<RoleExtension>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<RoleExtension>>()//AddRoles������RoleManager
                .AddUserManager<UserManager<JobSeeker>>()
                .AddSignInManager<SignInManager<JobSeeker>>();

            //Jwt
            services.AddOptions().Configure<JwtSettings>(_confirguration.GetSection("Jwt"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                //var JwtSettings=_confirguration.GetSection("Jwt").Get<JwtSettings>();
                //_confirguration["Jwt:SecretKey"]�Ϳ�����JwtSettings.SecretKey����
                var secretByte = Encoding.UTF8.GetBytes(_confirguration["Jwt:ScrKey"]);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _confirguration["Jwt:Issuer"],//Ҳ����д��_confirguration
                    ValidateAudience = true,
                    ValidAudience = _confirguration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                };
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
