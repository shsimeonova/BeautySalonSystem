using AutoMapper;
using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeautySalonSystem.Products.Models;

namespace BeautySalonSystem.Products.Profiles
{
    public class OffersProfile : Profile
    {
        public OffersProfile()
        {
            CreateMap<Offer, OfferDto>();
            CreateMap<Offer, GetOfferModel>();
            CreateMap<OfferDto, GetOfferModel>();
        }
    }
}
