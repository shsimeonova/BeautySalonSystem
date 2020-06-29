
namespace BeautySalonSystem.Services
{
    using AutoMapper;
    using BeautySalonSystem.Infrastructure;
    using BeautySalonSystem.Products.Data;
    using BeautySalonSystem.Products.Data.Repositories;
    using BeautySalonSystem.Products.Profiles;
    using BeautySalonSystem.Products.Services;
    using BeautySalonSystem.Profiles;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddWebService<ProductsDbContext>(this.Configuration)
                .AddTransient<IProductsRepository, ProductsRepository>()
                .AddTransient<IProductOffersRepository, ProductOffersRepository>()
                .AddTransient<IOffersRepository, OffersRepository>()
                .AddTransient<IProductOffersService, ProductOffersService>()
                .AddMessaging();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductsProfile());
                mc.AddProfile(new OffersProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .Initialize();
    }
}
