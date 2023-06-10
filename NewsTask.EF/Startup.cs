using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsTask.Core.Interfaces;
using NewsTask.Core.Repository;
using NewsTask.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsTask.EF
{
    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }));

            services.AddTransient(typeof(IAuthorServices), typeof(AuthorServices));

            services.AddTransient(typeof(INewsServices), typeof(NewsServices));
            services.AddScoped(typeof(IAuthenticationServices), typeof(AuthenticationServices));
        }

        public void Configure(IServiceProvider provider)
        {
            //RecurringJob.AddOrUpdate<INewsServices>("PublishNews", x => x.PublishToBePublished(), Cron.Daily);

        }


    }
}