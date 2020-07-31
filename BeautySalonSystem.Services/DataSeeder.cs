using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using BeautySalonSystem.Products.Data;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Data.Repositories;
using BeautySalonSystem.Products.Models.ProductOffer;
using BeautySalonSystem.Products.Profiles;
using BeautySalonSystem.Products.Services;
using BeautySalonSystem.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BeautySalonSystem.Products
{
    public interface IDataSeeder
    {
        public void EnsureSeedData(string connectionString);
    }
    public class DataSeeder : IDataSeeder
    {
        public void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(connectionString));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ProductsDbContext>();
                    context.Database.Migrate();
                    
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new ProductsProfile());
                        mc.AddProfile(new OffersProfile());
                    });

                    IMapper mapper = mappingConfig.CreateMapper();
                    var productsRepository = new ProductsRepository(context);

                    var offersService = new OffersService(
                        new OffersRepository(context), 
                        productsRepository, 
                        new ProductOffersRepository(context), 
                        mapper);
                    
                    var products = new ArrayList
                    {
                        new Product { Name = "Терапия с арганово масло", Type = ProductType.Hair, Price = 20, Duration = 30},
                        new Product { Name = "Подстригване с измиване и изсушаване", Type = ProductType.Hair, Price = 25, Duration = 50},
                        new Product { Name = "Кола маска", Type = ProductType.BodyHair, Price = 50, Duration = 60},
                        new Product { Name = "Лазерна епилация", Type = ProductType.BodyHair, Price = 100, Duration = 60},
                        new Product { Name = "Маникюр", Type = ProductType.Nails, Price = 25, Duration = 90}
                    };

                    Dictionary<string, int> productsDict = new Dictionary<string, int>();
                    foreach (Product product in products)
                    {
                        if (productsRepository.ExistsByName(product.Name))
                        {
                            continue;
                        }
                        context.Products.Add(product);
                        context.SaveChanges();
                        productsDict[product.Name] = product.Id;
                    }

                    var offers = new List<CreateProductOfferInputModel>
                    {
                        new CreateProductOfferInputModel
                        {
                            Name = "Подстригване + арганова терапия",
                            Discount = 0,
                            TotalPrice = 45,
                            Products = new[]
                            {
                                productsDict["Терапия с арганово масло"],
                                productsDict["Подстригване с измиване и изсушаване"]
                            },
                            ExpiryDate = "2020-08-29",
                            IsActive = true
                        },
                        new CreateProductOfferInputModel
                        {
                            Name = "Кола маска",
                            TotalPrice = 50,
                            Products = new[] {productsDict["Кола маска"]},
                            ExpiryDate = "2020-08-10",
                            IsActive = true
                        },
                        new CreateProductOfferInputModel
                        {
                            Name = "Цялостна грижа",
                            TotalPrice = 150,
                            Products = new[]
                            {
                                productsDict["Терапия с арганово масло"],
                                productsDict["Подстригване с измиване и изсушаване"], productsDict["Лазерна епилация"],
                                productsDict["Маникюр"]
                            },
                            ExpiryDate = "2020-09-30",
                            IsActive = true
                        }
                    };

                    foreach (var offer in offers)
                    {
                        try
                        {
                            offersService.Create(offer, "SYSTEM");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}
