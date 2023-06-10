using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsTask.Core.Helpers;
using NewsTask.Core.Repository;
using NewsTask.EF;
using NewsTask.EF.Migrations;
using NewsTask.EF.Repositories;
using System.Configuration;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Hangfire;
using Hangfire.SqlServer;
using NewsTask.Api.Filters;

namespace NewsTask.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly List<Core.Interfaces.IStartup> _assembliesStartup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _assembliesStartup = new List<Core.Interfaces.IStartup>
            {
                new EF.Startup(configuration)
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);

            services.Configure<JWT>(Configuration.GetSection("JWT"));
            services.AddControllers();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvcCore().AddAuthorization();

            services.AddSwaggerGen(
                 options =>
                 {
                     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                     {
                         In = ParameterLocation.Header,
                         Description = "Please insert JWT with Bearer into field",
                         Name = "Authorization",
                         Type = SecuritySchemeType.ApiKey,
                     });

                     options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                     });

                 });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            });

            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true,
               }));
            services.AddHangfireServer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            _assembliesStartup.ForEach(startup => startup.ConfigureServices(services));

            services.AddEndpointsApiExplorer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"C:\\Logs\\Log.txt", LogLevel.Information);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePages();

            app.UseHangfireDashboard(options: new DashboardOptions()
            {
                Authorization = new[] { new HangfireDashboardNoAuthFilter() }
            });

            RecurringJob.AddOrUpdate<INewsServices>("PublishNews", x => x.PublishToBePublished(), Cron.Daily);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //app.Run();
        }
    }
}
