using System;
using Microsoft.Extensions.DependencyInjection;

namespace NewsTask.Core.Interfaces
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);

        void Configure(IServiceProvider provider);
    }
}
