using BeautySalonSystem.Appointments.Data.Models;

namespace BeautySalonSystem.Appointments.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class AppointmentsDbContext : DbContext
    {
        public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options) : base(options) {}

        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
