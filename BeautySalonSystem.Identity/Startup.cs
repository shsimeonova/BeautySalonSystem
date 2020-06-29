namespace BeautySalonSystem.Identity
{
    using BeautySalonSystem.Infrastructure;
    using BeautySalonSystem.Services;
    using Data;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Identity;
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
            => services
                .AddWebService<IdentityDbContext>(this.Configuration)
                .Configure<IdentityDbContext>(o => o.Database.Migrate())
                .AddUserStorage()
                .AddTransient<IDataSeeder, IdentityDataSeeder>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<ITokenGeneratorService, TokenGeneratorService>();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .Initialize();
    }
}
