namespace BeautySalonSystem.Products.Data
{
    using BeautySalonSystem.Products.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Reflection;

    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<ProductOffer> ProductOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))) 
                {
                  property.SetColumnType("decimal(18, 6)");
                }

            builder.Entity<ProductOffer>()
                .HasKey(po => new { po.ProductId, po.OfferId });

            builder.Entity<ProductOffer>()
                .HasOne(p => p.Product)
                .WithMany(o => o.ProductOffers)
                .HasForeignKey(y => y.ProductId);

            builder.Entity<ProductOffer>()
                .HasOne(x => x.Offer)
                .WithMany(y => y.ProductOffers)
                .HasForeignKey(y => y.OfferId);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
