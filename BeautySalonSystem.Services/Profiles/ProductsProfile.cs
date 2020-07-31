using AutoMapper;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BeautySalonSystem.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, GetProductOutputModel>();
            CreateMap<CreateProductInputModel, Product>();
        }
    }
}