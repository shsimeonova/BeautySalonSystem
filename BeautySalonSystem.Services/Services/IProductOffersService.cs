using BeautySalonSystem.Products.Models.ProductOffer;
using BeautySalonSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Services
{
    public interface IProductOffersService
    {
        Task<Result<CreateProductOfferOutputModel>> Create(CreateProductOfferInputModel input);
        Task<Result<IEnumerable<ListProductOffersModel>>> GetAll();
        Task<Result<ListProductOffersModel>> GetAllByProductId(int productId);
    }
}
