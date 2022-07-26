using BriefResume.DataBase;
using BriefResume.IService;
using BriefResume.JwtModel;
using BriefResume.Models;
using BriefResume.Service;
using BriefResume.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Formatting.Json;
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
            //Dbcontext注入
            services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlServer(_confirguration["DbContext:ConnectionString"]);//得到的是string
            });

            //Jwt Authentic
            services.AddOptions().Configure<JwtSettings>(_confirguration.GetSection("Jwt"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //var JwtSettings=_confirguration.GetSection("Jwt").Get<JwtSettings>();
                    //_confirguration["Jwt:SecretKey"]就可以用JwtSettings.SecretKey代替
                    var secretByte = Encoding.UTF8.GetBytes(_confirguration["Jwt:ScrKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _confirguration["Jwt:Issuer"],//也可以写成_confirguration
                        ValidateAudience = true,
                        ValidAudience = _confirguration["Jwt:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });
            

            services.AddDataProtection();//密码加密服务
            services.AddIdentityCore<Seeker>(opt => //与AddIdentity不同,AddIdentity还会添加默认界面
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                //密码生成规则
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;//密码重置时,生成较为简单的密码
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                //用户名规则
                //opt.User.AllowedUserNameCharacters 这里可以用正则表达式设置规则
            });

            var idBuilder = new IdentityBuilder(typeof(Seeker), typeof(RoleExtension), services);
            idBuilder.AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<RoleExtension>>()
                .AddUserManager<SeekerManager>()
                .AddSignInManager<SignInManager<Seeker>>();

            //MVC配置
            services.AddControllers(setupAction => {
                setupAction.ReturnHttpNotAcceptable = true;//若为false则忽略请求头accept指定的格式
            })
                .AddNewtonsoftJson(setupAction =>
                { //若与下方xml写反，则当patch时会先默认使用xml
                    setupAction.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();//注册解析jsondocument
                    setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;//一对一关系中,避免循环选取
                })
                .AddXmlDataContractSerializerFormatters()//内容协商,可以使用Xml输出数据
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                setupAction.InvalidModelStateResponseFactory = context =>//数据验证失败时自定义报错信息
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "any",
                        Title = "数据验证失败",
                        Status = StatusCodes.Status422UnprocessableEntity,//指定验证失败返回422
                        Detail = "查看详细说明",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }//指定错误信息的格式
                    };
                };
                });

            //仓储配置
            services.AddTransient<IAblityRepository, AblityRepository>();
            services.AddTransient<IInterviewerAttributeRepository, InterviewerAttributeRepository>();
            services.AddTransient<IJobSeekerAttributeRepository, JobSeekerAttributeRepository>();
            //排序仓储
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            //automapper注入
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //缓存机制,不采用自动的服务器缓存,而是手动管理
            services.AddMemoryCache();

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
            
            //app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
