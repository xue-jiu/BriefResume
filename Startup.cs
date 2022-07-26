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
            //Dbcontextע��
            services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlServer(_confirguration["DbContext:ConnectionString"]);//�õ�����string
            });

            //Jwt Authentic
            services.AddOptions().Configure<JwtSettings>(_confirguration.GetSection("Jwt"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
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
            

            services.AddDataProtection();//������ܷ���
            services.AddIdentityCore<Seeker>(opt => //��AddIdentity��ͬ,AddIdentity�������Ĭ�Ͻ���
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                //�������ɹ���
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;//��������ʱ,���ɽ�Ϊ�򵥵�����
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                //�û�������
                //opt.User.AllowedUserNameCharacters ���������������ʽ���ù���
            });

            var idBuilder = new IdentityBuilder(typeof(Seeker), typeof(RoleExtension), services);
            idBuilder.AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<RoleExtension>>()
                .AddUserManager<SeekerManager>()
                .AddSignInManager<SignInManager<Seeker>>();

            //MVC����
            services.AddControllers(setupAction => {
                setupAction.ReturnHttpNotAcceptable = true;//��Ϊfalse���������ͷacceptָ���ĸ�ʽ
            })
                .AddNewtonsoftJson(setupAction =>
                { //�����·�xmlд������patchʱ����Ĭ��ʹ��xml
                    setupAction.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();//ע�����jsondocument
                    setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;//һ��һ��ϵ��,����ѭ��ѡȡ
                })
                .AddXmlDataContractSerializerFormatters()//����Э��,����ʹ��Xml�������
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                setupAction.InvalidModelStateResponseFactory = context =>//������֤ʧ��ʱ�Զ��屨����Ϣ
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "any",
                        Title = "������֤ʧ��",
                        Status = StatusCodes.Status422UnprocessableEntity,//ָ����֤ʧ�ܷ���422
                        Detail = "�鿴��ϸ˵��",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }//ָ��������Ϣ�ĸ�ʽ
                    };
                };
                });

            //�ִ�����
            services.AddTransient<IAblityRepository, AblityRepository>();
            services.AddTransient<IInterviewerAttributeRepository, InterviewerAttributeRepository>();
            services.AddTransient<IJobSeekerAttributeRepository, JobSeekerAttributeRepository>();
            //����ִ�
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            //automapperע��
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //�������,�������Զ��ķ���������,�����ֶ�����
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
