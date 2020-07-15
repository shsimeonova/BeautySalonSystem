using System;
using System.Collections.Generic;
using AutoMapper;
using BeautySalonSystem.Products.Data;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Data.Repositories;
using BeautySalonSystem.Products.Models;
using BeautySalonSystem.Products.Models.ProductOffer;

namespace BeautySalonSystem.Products.Services
{
    public interface IOffersService
    {
        public IEnumerable<GetOfferModel> GetAll();
        public int Create(CreateProductOfferInputModel input, string currentUserId);
    } 
    
    public class OffersService : IOffersService
    {
        private IOffersRepository _offersRepository;
        private IProductsRepository _productsRepository;
        private IProductOffersRepository _productOffersRepository;
        private readonly IMapper _mapper;

        public OffersService(
            IOffersRepository offersRepository, 
            IProductsRepository productsRepository,
            IProductOffersRepository productOffersRepository,
            IMapper mapper)
        {
            _offersRepository = offersRepository;
            _productsRepository = productsRepository;
            _productOffersRepository = productOffersRepository;
            _mapper = mapper;
        }

        public IEnumerable<GetOfferModel> GetAll()
        {
            IEnumerable<OfferDto> allOffers = _offersRepository.GetAll();
            List<GetOfferModel> result = new List<GetOfferModel>();
            foreach (var offer in allOffers)
            {
                GetOfferModel mappedOffer = _mapper.Map<OfferDto, GetOfferModel>(offer);
                result.Add(mappedOffer);
                Console.WriteLine();
            }

            return result;
        }

        public int Create(CreateProductOfferInputModel input, string currentUserId)
        {
            IEnumerable<Product> selectedProducts = _productsRepository.GetByIds(input.Products);
            
            Offer offer = new Offer
            {
                Name = input.Name,
                TotalPrice = input.TotalPrice,
                Discount = input.Discount,
                ExpiryDate = DateTime.ParseExact(input.ExpiryDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                AddedById = currentUserId
            };
            
            _offersRepository.Add(offer);
            _offersRepository.SaveChanges();
            
            foreach (var selectedProduct in selectedProducts)
            {
                ProductOffer po = new ProductOffer
                {
                    OfferId = offer.Id,
                    ProductId = selectedProduct.Id
                };
                
                _productOffersRepository.Add(po);
                _productOffersRepository.SaveChanges();
            }

            return offer.Id;
        }
    }
}