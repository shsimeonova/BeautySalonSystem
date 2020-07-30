using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using BeautySalonSystem.UI.Util;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BeautySalonSystem.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var SecretKey = this.Configuration.GetSection("ApplicationSettings:Secret").Value;
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", options =>
                {
                    options.ExpireTimeSpan = new TimeSpan(0, 2, 0);
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:8001";
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "BeautySalonSystem.UI";
                    options.ClientSecret = SecretKey;
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.Scope.Add("role");
                    options.Scope.Add("email");
                    options.Scope.Add("ms");
                    options.SaveTokens = true;
                    options.UseTokenLifetime = true;
                    options.ClaimActions.MapJsonKey("role", "role");
                    options.ClaimActions.MapJsonKey("email", "email");
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Events.OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.Prompt = "login";
                        if (context.Properties.Items.ContainsKey("action"))
                        {
                            context.ProtocolMessage.SetParameter("action", context.Properties.Items["action"]);
                        }
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("role", "Admin"));
            });
            
            services.AddSingleton<ISessionHelper, SessionHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient<IAppointmentsService, AppointmentsService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("Services:Appointments:Url").Value + "appointments");
            });
            services.AddHttpClient<IOffersService, OffersService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("Services:Products:Url").Value + "offers");
            });
            services.AddHttpClient<IProductsService, ProductsService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("Services:Products:Url").Value + "products");
            });
            services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("Services:Identity:Url").Value);
            });

            services.AddTransient<IAppointmentsBusinessService, AppointmentsBusinessService>();

            services.AddMassTransit(options =>
            {
                options.AddBus(context => Bus.Factory.CreateUsingRabbitMq(config => config.Host("rabbitmq://localhost")));
            }).AddMassTransitHostedService();
            
            IMapper mapper = InitAutoMapper();
            services.AddSingleton(mapper);

            services.AddRazorPages();
        }
        
        public Mapper InitAutoMapper()
        {
            var mapperConfig = new MapperConfiguration(options => options.CreateMap<AppointmentViewModel, AppointmentAdminViewModel>());
            
            var mapper = new Mapper(mapperConfig);
            return mapper;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
