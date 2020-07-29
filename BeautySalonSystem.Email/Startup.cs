using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalonSystem.Email
{
    using Email_AppointmentConfirmedConsumer = Email.AppointmentConfirmedConsumer;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            
            services.AddMassTransit(options =>
            {
                options.AddConsumer<Email_AppointmentConfirmedConsumer>();
                options.AddBus(context => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host("rabbitmq://localhost");
                    config.ReceiveEndpoint(nameof(AppointmentConfirmedConsumer), endpoint =>
                    {
                        endpoint.ConfigureConsumer<AppointmentConfirmedConsumer>(context);
                    });
                }));
            }).AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}