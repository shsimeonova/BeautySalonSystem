using AutoMapper;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Data.Repositories;
using BeautySalonSystem.Products.Models;
using BeautySalonSystem.Products.Models.ProductOffer;
using BeautySalonSystem.Services;
using BeautySalonSystem.Services.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Services
{
    public class ProductOffersService : IProductOffersService
    {
        private readonly IProductOffersRepository _productOffersRepository;
        private readonly IProductsRepository _productRepository;
        private readonly IOffersRepository _offersRepository;
        private readonly IMapper _mapper;

        public ProductOffersService(IProductOffersRepository productOffersRepository,
           IProductsRepository productRepository,
           IOffersRepository offersRepository,
           IMapper mapper)
        {
            _productOffersRepository = productOffersRepository;
            _productRepository = productRepository;
            _offersRepository = offersRepository;
            _mapper = mapper;
        }

        public async Task<Result<CreateProductOfferOutputModel>> Create(CreateProductOfferInputModel input)
        {
            List<Product> products = new List<Product>();
            foreach (int id in input.Products)
            {
                Product product = _productRepository.GetByID(id);

                if (product == null)
                {
                    return string.Format(Constants.ProductNotFound, id);
                }

                products.Add(product);
            }

            var offer = new Offer
            {
                Name = input.Name,
                TotalPrice = products.Sum(p => p.Price),
                Discount = input.Discount,
                ExpiryDate = DateTime.ParseExact(input.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                ProductOffers = new List<ProductOffer>()
            };

            _offersRepository.Add(offer);
            _offersRepository.SaveChanges();

            products.ForEach(p =>
            {
                var productOffer = new ProductOffer()
                {
                    ProductId = p.Id,
                    OfferId = offer.Id
                };

                this._productOffersRepository.Add(productOffer);
                this._productOffersRepository.SaveChanges();
            });

            return new CreateProductOfferOutputModel() { OfferId = offer.Id };
        }

        public Task<Result<IEnumerable<ListProductOffersModel>>> GetAll()
        {
            return _offersRepository.GetAll()
        }

        public async Task<Result<ListProductOffersModel>> GetAllByProductId(int productId)
        {
            var productOffers = this._productOffersRepository.GetAllByProductId(productId);

            if (!productOffers.Any())
            {
                return Constants.NoProductOffersForProductId;
            }

            ListProductOffersModel result = new ListProductOffersModel();
            productOffers.ForEach(po => {
                var offer = this._offersRepository.GetByID(po.OfferId);
                result.Offers.Add(_mapper.Map<Offer, GetOfferModel>(offer));
            });

            return result;
        }
    }
}
