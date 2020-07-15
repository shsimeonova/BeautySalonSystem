using System.IdentityModel.Tokens.Jwt;
using BeautySalonSystem.Products.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BeautySalonSystem.Products
{
    using AutoMapper;
    using Data;
    using Data.Repositories;
    using Profiles;
    using BeautySalonSystem.Profiles;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var connStr = this.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            services
                .AddDbContext<ProductsDbContext>(opt => opt.UseSqlServer(connStr))
                .AddTransient<IProductsRepository, ProductsRepository>()
                .AddTransient<IProductOffersRepository, ProductOffersRepository>()
                .AddTransient<IOffersRepository, OffersRepository>()
                .AddTransient<IOffersService, OffersService>();


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductsProfile());
                mc.AddProfile(new OffersProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            var SecretKey = this.Configuration.GetSection("ApplicationSettings:Secret").Value;

            services.AddAuthorization();
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:8001";
                    options.Audience = "ms";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false
                    };
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
