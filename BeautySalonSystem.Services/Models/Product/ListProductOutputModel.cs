using AutoMapper;
using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Models
{
    public class ListProductsOutputModel
    {
        public IEnumerable<GetProductOutputModel> products;
    }
}
