using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonSystem.Identity.Data
{
    using BeautySalonSystem.Identity.Data.Models;
    public class IdentityDbContext : IdentityDbContext<User>
        {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
