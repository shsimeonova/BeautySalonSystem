using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<GetOfferModel> GetAll(bool activeOnly);
        public GetOfferModel GetById(int id);
        public IEnumerable<GetOfferModel> GetManyByIds(bool activeOnly, int[] id);
        public void Activate(int id);
        public int GetTotalDuration(int id);
        public int Create(CreateProductOfferInputModel input, string currentUserId);
        void Delete(int id);
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

        public IEnumerable<GetOfferModel> GetAll(bool activeOnly)
        {
            IEnumerable<OfferDto> allOffers = _offersRepository.GetAll(activeOnly);
            List<GetOfferModel> result = new List<GetOfferModel>();
            foreach (var offer in allOffers)
            {
                GetOfferModel mappedOffer = _mapper.Map<OfferDto, GetOfferModel>(offer);
                result.Add(mappedOffer);
            }

            return result;
        }

        public GetOfferModel GetById(int id)
        {
            Offer offer = _offersRepository.GetById(id);
            var result = new OfferDto()
            {
                Id = offer.Id,
                Name = offer.Name,
                Discount = offer.Discount,
                TotalPrice = offer.TotalPrice,
                ExpiryDate = offer.ExpiryDate,
                AddedById = offer.AddedById,
                Products = _productsRepository.GetAllByOffer(offer),
                IsActive = offer.IsActive
            };
            
            return _mapper.Map<OfferDto, GetOfferModel>(result);
        }

        public IEnumerable<GetOfferModel> GetManyByIds(bool activeOnly, int[] ids)
        {
            var allOffers = this._offersRepository.GetAllByIds(activeOnly, ids);
            List<GetOfferModel> result = new List<GetOfferModel>();
            foreach (var offer in allOffers)
            {
                GetOfferModel mappedOffer = _mapper.Map<OfferDto, GetOfferModel>(offer);
                result.Add(mappedOffer);
            }

            return result;
        }

        public void Activate(int id)
        {
            var offer = _offersRepository.GetById(id);

            if (offer == null)
            {
                throw new ArgumentNullException();
            }

            offer.IsActive = true;
            _offersRepository.Update(offer);
            _offersRepository.SaveChanges();
        }

        public int GetTotalDuration(int id)
        {
            var offer = _offersRepository.GetById(id);
            if (offer == null)
            {
                throw new ArgumentNullException();
            }
            
            int duration = offer.ProductOffers.Select(po => po.Product).Sum(product => product.Duration);
            return duration;
        }

        public int Create(CreateProductOfferInputModel input, string currentUserId)
        {
            IEnumerable<Product> selectedProducts = _productsRepository.GetByIds(input.Products);
            
            Offer offer = new Offer
            {
                Name = input.Name,
                TotalPrice = decimal.Round(input.TotalPrice),
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

        public void Delete(int id)
        {
            var offer = _offersRepository.GetById(id);

            if (offer == null)
            {
                throw new ArgumentNullException();
            }
            
            _offersRepository.Delete(offer);
            _offersRepository.SaveChanges();
        }
    }
}